using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class JobAcceptedDomainEventHandler
    : INotificationHandler<JobAcceptedDomainEvent>
{
    private readonly ILogger<JobAcceptedDomainEventHandler> _logger;

    public JobAcceptedDomainEventHandler(
        ILogger<JobAcceptedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        JobAcceptedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Artisan {ArtisanId} accepted assignment {AssignmentId}.",
            notification.ArtisanId,
            notification.AssignmentId);

        return Task.CompletedTask;
    }
}