using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class JobRejectedDomainEvent : DomainEvent
{
    public Guid AssignmentId { get; }

    public Guid ServiceRequestId { get; }

    public Guid ArtisanId { get; }

    public JobRejectedDomainEvent(
        Guid assignmentId,
        Guid serviceRequestId,
        Guid artisanId)
    {
        AssignmentId = assignmentId;
        ServiceRequestId = serviceRequestId;
        ArtisanId = artisanId;
    }
}