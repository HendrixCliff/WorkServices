using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Entities;

public class JobAssignment : Entity
{
    private JobAssignment()
    {
    }

    public Guid ServiceRequestId { get; private set; }

    public Guid ArtisanId { get; private set; }

    public DateTime AssignedAt { get; private set; }

    public bool Accepted { get; private set; }

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
        Accepted = true;
        MarkUpdated();
    }

    public void Reject()
    {
        Accepted = false;
        MarkUpdated();
    }
}