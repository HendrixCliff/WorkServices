using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class QuoteApprovedDomainEventHandler
    : INotificationHandler<QuoteApprovedDomainEvent>
{
    private readonly ILogger<QuoteApprovedDomainEventHandler> _logger;

    public QuoteApprovedDomainEventHandler(
        ILogger<QuoteApprovedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        QuoteApprovedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Quote approved for request {RequestId}.",
            notification.ServiceRequestId);

        return Task.CompletedTask;
    }
}