using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.Payments.Commands.InitializePayment;
using WorkServices.Application.Features.Payments.Commands.MarkPaymentSuccessful;
using WorkServices.Application.Features.Payments.Queries.GetPaymentsByServiceRequest;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/payments")]
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

    [HttpPost("{paymentId:guid}/initialize")]
    public async Task<IActionResult> Initialize(Guid paymentId)
    {
        var url = await _mediator.Send(
            new InitializePaymentCommand(paymentId));

        return Ok(new
        {
            AuthorizationUrl = url
        });
    }

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