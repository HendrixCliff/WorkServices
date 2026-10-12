
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WorkServices.Application.Features.JobAssignments.Queries.GetArtisanJobs;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/artisans")]
[Authorize(Roles = "Artisan")]
[EnableRateLimiting("ApiPolicy")]
public sealed class ArtisansController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArtisansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("jobs")]
    [Authorize(Policy = "ArtisanOnly")]
    public async Task<IActionResult> GetJobs(
        CancellationToken cancellationToken)
    {
        var artisanIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(artisanIdValue, out var artisanId))
        {
            return Unauthorized();
        }

        var jobs = await _mediator.Send(
            new GetArtisanJobsQuery(artisanId),
            cancellationToken);

        return Ok(jobs);
    }
}
