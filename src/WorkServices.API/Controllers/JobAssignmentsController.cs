using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.JobAssignments.Commands.AcceptJob;
using WorkServices.Application.Features.JobAssignments.Commands.AssignJob;
using WorkServices.Application.Features.JobAssignments.Commands.CompleteJob;
using WorkServices.Application.Features.JobAssignments.Commands.RejectJob;
using WorkServices.Application.Features.JobAssignments.Commands.StartJob;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/job-assignments")]
[EnableRateLimiting("ApiPolicy")]
public sealed class JobAssignmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobAssignmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

  
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Assign(
        AssignJobCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

   
    [Authorize(Policy = "ArtisanOnly")]
    [HttpPost("{assignmentId:guid}/accept")]
    public async Task<IActionResult> Accept(Guid assignmentId)
    {
        await _mediator.Send(
            new AcceptJobCommand(assignmentId));

        return NoContent();
    }

    
    [Authorize(Policy = "ArtisanOnly")]
    [HttpPost("{assignmentId:guid}/reject")]
    public async Task<IActionResult> Reject(Guid assignmentId)
    {
        await _mediator.Send(
            new RejectJobCommand(assignmentId));

        return NoContent();
    }

    
    [Authorize(Policy = "ArtisanOnly")]
    [HttpPost("{serviceRequestId:guid}/start")]
    public async Task<IActionResult> Start(Guid serviceRequestId)
    {
        var artisanId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _mediator.Send(
            new StartJobCommand(
                serviceRequestId,
                artisanId));

        return NoContent();
    }

  
    [Authorize(Policy = "ArtisanOnly")]
    [HttpPost("{serviceRequestId:guid}/complete")]
    public async Task<IActionResult> Complete(Guid serviceRequestId)
    {
        var artisanId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _mediator.Send(
            new CompleteJobCommand(
                serviceRequestId,
                artisanId));

        return NoContent();
    }
}