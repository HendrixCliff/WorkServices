using WorkServices.Domain.Enums;
using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class JobCompletedDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public Guid CustomerId { get; }

    public Guid ArtisanId { get; }

    public ServiceType ServiceType { get; }

    public JobCompletedDomainEvent(
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