using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class MaterialsPaidDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public Guid PaymentId { get; }

    public MaterialsPaidDomainEvent(
        Guid serviceRequestId,
        Guid paymentId)
    {
        ServiceRequestId = serviceRequestId;
        PaymentId = paymentId;
    }
}