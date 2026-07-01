using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class JobAcceptedDomainEvent : DomainEvent
{
    public Guid AssignmentId { get; }

    public Guid ServiceRequestId { get; }

    public Guid ArtisanId { get; }

    public JobAcceptedDomainEvent(
        Guid assignmentId,
        Guid serviceRequestId,
        Guid artisanId)
    {
        AssignmentId = assignmentId;
        ServiceRequestId = serviceRequestId;
        ArtisanId = artisanId;
    }
}