using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class LabourPaymentCreatedDomainEvent
    : DomainEvent
{
    public Guid PaymentId { get; }

    public Guid ServiceRequestId { get; }

    public decimal Amount { get; }

    public LabourPaymentCreatedDomainEvent(
        Guid paymentId,
        Guid serviceRequestId,
        decimal amount)
    {
        PaymentId = paymentId;
        ServiceRequestId = serviceRequestId;
        Amount = amount;
    }
}