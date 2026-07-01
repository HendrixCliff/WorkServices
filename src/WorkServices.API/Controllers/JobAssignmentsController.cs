using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.JobAssignments.Commands.AcceptJob;
using WorkServices.Application.Features.JobAssignments.Commands.AssignJob;
using WorkServices.Application.Features.JobAssignments.Commands.CompleteJob;
using WorkServices.Application.Features.JobAssignments.Commands.RejectJob;
using WorkServices.Application.Features.JobAssignments.Commands.StartJob;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/job-assignments")]
public sealed class JobAssignmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobAssignmentsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Assign(
        AssignJobCommand command)
    {
        var id =
            await _mediator.Send(command);

        return Ok(id);
    }

    [HttpPost("{assignmentId:guid}/accept")]
    public async Task<IActionResult> Accept(
        Guid assignmentId)
    {
        await _mediator.Send(
            new AcceptJobCommand(assignmentId));

        return NoContent();
    }

    [HttpPost("{assignmentId:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid assignmentId)
    {
        await _mediator.Send(
            new RejectJobCommand(assignmentId));

        return NoContent();
    }

   

    [HttpPost("{serviceRequestId:guid}/start")]
    public async Task<IActionResult> Start(
        Guid serviceRequestId)
    {
        var artisanId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _mediator.Send(
            new StartJobCommand(
                serviceRequestId,
                artisanId));

        return NoContent();
    }

    [HttpPost("{serviceRequestId:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid serviceRequestId)
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