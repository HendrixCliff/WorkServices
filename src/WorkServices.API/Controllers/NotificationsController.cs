using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.Notification.Queries.GetNotifications;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/notifications")]
public sealed class NotificationsController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(
        Guid userId)
    {
        return Ok(
            await _mediator.Send(
                new GetNotificationsQuery(
                    userId)));
    }
}