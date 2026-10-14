using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.Payments.Commands.InitializePayment;
using WorkServices.Application.Features.Payments.Commands.MarkPaymentSuccessful;
using WorkServices.Application.Features.Payments.Queries.GetPaymentsByServiceRequest;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using WorkServices.Domain.Enums;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/payments")]
[EnableRateLimiting("ApiPolicy")]
public sealed class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPaystackService _paystack;
    private readonly IPaymentRepository _payments;

    public PaymentsController(
        IMediator mediator,
        IPaystackService paystack,
        IPaymentRepository payments)
    {
        _mediator = mediator;
        _paystack = paystack;
        _payments = payments;
    }
    
    [Authorize(Policy = "CustomerOnly")]
    [HttpPost("service-request/{serviceRequestId:guid}/initialize")]
    public async Task<IActionResult> Initialize(
        Guid serviceRequestId,
        [FromQuery] PaymentType paymentType)
    {
        if (!Enum.IsDefined(paymentType))
        {
            return BadRequest("Invalid payment type.");
        }

        var url = await _mediator.Send(
            new InitializePaymentCommand(
                serviceRequestId,
                paymentType));

        return Ok(new
        {
            AuthorizationUrl = url
        });
    }

    [Authorize(Policy = "CustomerOnly")]
    [HttpGet("verify")]
    public async Task<IActionResult> Verify(
        [FromQuery] string reference)
    {
        var payment =
            await _payments.GetByReferenceAsync(reference);

        if (payment is null)
            return NotFound();

        var success =
            await _paystack.VerifyPaymentAsync(reference);

        if (!success)
            return BadRequest("Payment not successful.");

        await _mediator.Send(
            new MarkPaymentSuccessfulCommand(
                payment.Id,
                reference));

        return Ok(new
        {
            Message = "Payment successful."
        });
    }

    [HttpGet("service-request/{serviceRequestId}")]
    public async Task<IActionResult> Get(Guid serviceRequestId)
    {
        return Ok(
            await _mediator.Send(
                new GetPaymentsByServiceRequestQuery(
                    serviceRequestId)));
    }
}