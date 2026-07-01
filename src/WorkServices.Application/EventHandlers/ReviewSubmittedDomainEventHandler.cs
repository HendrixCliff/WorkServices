using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class ReviewSubmittedDomainEventHandler
    : INotificationHandler<ReviewSubmittedDomainEvent>
{
    private readonly ILogger<ReviewSubmittedDomainEventHandler> _logger;

    public ReviewSubmittedDomainEventHandler(
        ILogger<ReviewSubmittedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        ReviewSubmittedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            """
            Review submitted.

            Request={Request}

            Artisan={Artisan}

            Rating={Rating}
            """,
            notification.ServiceRequestId,
            notification.ArtisanId,
            notification.Rating);

        return Task.CompletedTask;
    }
}