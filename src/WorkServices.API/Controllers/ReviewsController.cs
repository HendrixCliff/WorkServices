using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.Reviews.Commands.CreateReview;
using WorkServices.Application.Features.Reviews.Queries.GetReviewsByArtisan;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/reviews")]
public sealed class ReviewsController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateReviewCommand command)
    {
        var id =
            await _mediator.Send(command);

        return Ok(id);
    }

    [HttpGet("artisan/{artisanId}")]
    public async Task<IActionResult> GetByArtisan(
        Guid artisanId)
    {
        return Ok(
            await _mediator.Send(
                new GetReviewsByArtisanQuery(
                    artisanId)));
    }
}