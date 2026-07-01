using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Events;

public sealed class JobAssignedDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public Guid CustomerId { get; }

    public Guid ArtisanId { get; }

    public ServiceType ServiceType { get; }

    public JobAssignedDomainEvent(
        Guid serviceRequestId,
        Guid customerId,
        Guid artisanId,
        ServiceType serviceType)
    {
        ServiceRequestId = serviceRequestId;
        CustomerId = customerId;
        ArtisanId = artisanId;
        ServiceType = serviceType;
    }
}