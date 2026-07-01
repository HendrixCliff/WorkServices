using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class JobCompletedDomainEventHandler
    : INotificationHandler<JobCompletedDomainEvent>
{
    private readonly ILogger<JobCompletedDomainEventHandler> _logger;

    public JobCompletedDomainEventHandler(
        ILogger<JobCompletedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        JobCompletedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            """
            Job completed.

            Request: {Request}

            Customer: {Customer}

            Artisan: {Artisan}

            Service: {Service}
            """,
            notification.ServiceRequestId,
            notification.CustomerId,
            notification.ArtisanId,
            notification.ServiceType);

        return Task.CompletedTask;
    }
}