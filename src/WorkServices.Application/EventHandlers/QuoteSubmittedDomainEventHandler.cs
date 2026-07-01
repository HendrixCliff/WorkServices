using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class QuoteSubmittedDomainEventHandler
    : INotificationHandler<QuoteSubmittedDomainEvent>
{
    private readonly ILogger<QuoteSubmittedDomainEventHandler> _logger;

    public QuoteSubmittedDomainEventHandler(
        ILogger<QuoteSubmittedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(
        QuoteSubmittedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Quote submitted for request {RequestId}.",
            notification.ServiceRequestId);

        return Task.CompletedTask;
    }
}