using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.ServiceRequests.Queries.GetServiceRequestsForAssignment;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public sealed class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("service-requests")]
    public async Task<IActionResult> GetServiceRequests()
    {
        var requests =
            await _mediator.Send(
                new GetServiceRequestsForAssignmentQuery());

        return Ok(requests);
    }
}