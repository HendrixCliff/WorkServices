using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class JobStartedDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public JobStartedDomainEvent(
        Guid serviceRequestId)
    {
        ServiceRequestId = serviceRequestId;
    }
}