using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class QuoteApprovedDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public QuoteApprovedDomainEvent(
        Guid serviceRequestId)
    {
        ServiceRequestId = serviceRequestId;
    }
}