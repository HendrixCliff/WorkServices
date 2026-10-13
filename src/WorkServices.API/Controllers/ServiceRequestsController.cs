
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WorkServices.Application.Features.ServiceRequests.Commands.CreateServiceRequest;
using WorkServices.Application.Features.ServiceRequests.Queries.GetMyRequests;
using WorkServices.Application.Features.ServiceRequests.Queries.GetServiceRequestById;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/service-requests")]
[EnableRateLimiting("ApiPolicy")]
public sealed class ServiceRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Create(
        [FromBody] CreateServiceRequestCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    [HttpGet("my")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyRequests(
        CancellationToken cancellationToken)
    {
        var customerIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(customerIdValue, out var customerId))
        {
            return Unauthorized();
        }

        var requests = await _mediator.Send(
            new GetMyRequestsQuery(customerId),
            cancellationToken);

        return Ok(requests);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = await _mediator.Send(
            new GetServiceRequestByIdQuery(id),
            cancellationToken);

        if (request is null)
        {
            return NotFound();
        }

        return Ok(request);
    }
}
