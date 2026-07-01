using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class JobRejectedDomainEventHandler
    : INotificationHandler<JobRejectedDomainEvent>
{
    private readonly ILogger<JobRejectedDomainEventHandler> _logger;

    public JobRejectedDomainEventHandler(
        ILogger<JobRejectedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        JobRejectedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Artisan {ArtisanId} rejected assignment {AssignmentId}.",
            notification.ArtisanId,
            notification.AssignmentId);

        return Task.CompletedTask;
    }
}