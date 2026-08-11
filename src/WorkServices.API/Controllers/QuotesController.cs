using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.Quotes.Commands.ApproveQuote;
using WorkServices.Application.Features.Quotes.Commands.CreateQuote;
using WorkServices.Application.Features.Quotes.Queries.GetQuoteByServiceRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/quotes")]
[EnableRateLimiting("ApiPolicy")]
public sealed class QuotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "ArtisanOnly")]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateQuoteCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    [Authorize(Policy = "CustomerOnly")]
    [HttpPost("{quoteId}/approve")]
    public async Task<IActionResult> Approve(
        Guid quoteId)
    {
        await _mediator.Send(
            new ApproveQuoteCommand(quoteId));

        return NoContent();
    }

    [Authorize]
    [HttpGet("service-request/{serviceRequestId}")]
    public async Task<IActionResult> Get(
        Guid serviceRequestId)
    {
        return Ok(
            await _mediator.Send(
                new GetQuoteByServiceRequestQuery(
                    serviceRequestId)));
    }
}