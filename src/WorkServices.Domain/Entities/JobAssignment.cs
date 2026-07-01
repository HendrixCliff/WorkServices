using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Events;
using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public class JobAssignment : Entity
{
    private JobAssignment()
    {
    }

    public Guid ServiceRequestId { get; private set; }

    public Guid ArtisanId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Pending;
    
    public ServiceRequest? ServiceRequest { get; private set; }

    public Artisan? Artisan { get; private set; }

  public JobAssignment(
    Guid serviceRequestId,
    Guid artisanId)
{
    ServiceRequestId = serviceRequestId;
    ArtisanId = artisanId;
    AssignedAt = DateTime.UtcNow;

}

    public void Accept()
    {
        if (Status != AssignmentStatus.Pending)
            throw new InvalidOperationException(
                "Only pending assignments can be accepted.");

        Status = AssignmentStatus.Accepted;

        AddDomainEvent(
            new JobAcceptedDomainEvent(
                Id,
                ServiceRequestId,
                ArtisanId));

        MarkUpdated();
    }

    public void Reject()
    {
        if (Status != AssignmentStatus.Pending)
            throw new InvalidOperationException(
                "Only pending assignments can be rejected.");

        Status = AssignmentStatus.Rejected;

        AddDomainEvent(
            new JobRejectedDomainEvent(
                Id,
                ServiceRequestId,
                ArtisanId));

        MarkUpdated();
    }
}