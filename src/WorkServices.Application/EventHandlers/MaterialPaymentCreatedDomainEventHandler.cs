using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class MaterialPaymentCreatedDomainEventHandler
    : INotificationHandler<MaterialPaymentCreatedDomainEvent>
{
    private readonly ILogger<MaterialPaymentCreatedDomainEventHandler> _logger;

    public MaterialPaymentCreatedDomainEventHandler(
        ILogger<MaterialPaymentCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        MaterialPaymentCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Material payment created. Payment={PaymentId} Amount={Amount}",
            notification.PaymentId,
            notification.Amount);

        return Task.CompletedTask;
    }
}