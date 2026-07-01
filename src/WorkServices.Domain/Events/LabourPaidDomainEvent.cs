using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class LabourPaidDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public Guid PaymentId { get; }

    public LabourPaidDomainEvent(
        Guid serviceRequestId,
        Guid paymentId)
    {
        ServiceRequestId = serviceRequestId;
        PaymentId = paymentId;
    }
}