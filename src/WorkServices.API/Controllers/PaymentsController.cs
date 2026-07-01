using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.Payments.Commands.PayForLabour;
using WorkServices.Application.Features.Payments.Commands.PayForMaterials;
using WorkServices.Application.Features.Payments.Queries.GetPaymentsByServiceRequest;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("materials")]
    public async Task<IActionResult> PayMaterials(
        PayForMaterialsCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPost("labour")]
    public async Task<IActionResult> PayLabour(
        PayForLabourCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet("service-request/{serviceRequestId}")]
    public async Task<IActionResult> Get(
        Guid serviceRequestId)
    {
        return Ok(
            await _mediator.Send(
                new GetPaymentsByServiceRequestQuery(
                    serviceRequestId)));
    }
}