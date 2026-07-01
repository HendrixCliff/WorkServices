using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class MaterialsPaidDomainEventHandler
    : INotificationHandler<MaterialsPaidDomainEvent>
{
    private readonly ILogger<MaterialsPaidDomainEventHandler> _logger;

    public MaterialsPaidDomainEventHandler(
        ILogger<MaterialsPaidDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        MaterialsPaidDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Materials paid for request {RequestId}.",
            notification.ServiceRequestId);

        return Task.CompletedTask;
    }
}