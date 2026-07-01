using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class LabourPaidDomainEventHandler
    : INotificationHandler<LabourPaidDomainEvent>
{
    private readonly ILogger<LabourPaidDomainEventHandler> _logger;

    public LabourPaidDomainEventHandler(
        ILogger<LabourPaidDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        LabourPaidDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Labour payment received for request {RequestId}.",
            notification.ServiceRequestId);

        return Task.CompletedTask;
    }
}