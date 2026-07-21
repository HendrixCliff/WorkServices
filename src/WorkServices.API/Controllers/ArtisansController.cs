using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.JobAssignments.Queries.GetArtisanJobs;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/artisans")]
[Authorize(Roles = "Artisan")]
public sealed class ArtisansController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArtisansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("jobs")]
    public async Task<IActionResult> GetJobs()
    {
        var artisanId = Guid.Parse(
            User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        var jobs =
            await _mediator.Send(
                new GetArtisanJobsQuery(
                    artisanId));

        return Ok(jobs);
    }
}