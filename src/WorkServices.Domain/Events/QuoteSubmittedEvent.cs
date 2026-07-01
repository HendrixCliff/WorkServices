using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class QuoteSubmittedDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public QuoteSubmittedDomainEvent(
        Guid serviceRequestId)
    {
        ServiceRequestId = serviceRequestId;
    }
}