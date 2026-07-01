using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class JobStartedDomainEventHandler
    : INotificationHandler<JobStartedDomainEvent>
{
    private readonly ILogger<JobStartedDomainEventHandler> _logger;

    public JobStartedDomainEventHandler(
        ILogger<JobStartedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        JobStartedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Job started for request {RequestId}.",
            notification.ServiceRequestId);

        return Task.CompletedTask;
    }
}