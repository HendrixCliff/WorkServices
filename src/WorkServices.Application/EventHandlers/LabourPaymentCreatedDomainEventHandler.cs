using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class LabourPaymentCreatedDomainEventHandler
    : INotificationHandler<LabourPaymentCreatedDomainEvent>
{
    private readonly ILogger<LabourPaymentCreatedDomainEventHandler> _logger;

    public LabourPaymentCreatedDomainEventHandler(
        ILogger<LabourPaymentCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        LabourPaymentCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Labour payment created. Amount={Amount}",
            notification.Amount);

        return Task.CompletedTask;
    }
}