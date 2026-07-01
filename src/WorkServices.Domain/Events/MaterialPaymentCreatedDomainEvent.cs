using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class MaterialPaymentCreatedDomainEvent
    : DomainEvent
{
    public Guid PaymentId { get; }

    public Guid ServiceRequestId { get; }

    public decimal Amount { get; }

    public MaterialPaymentCreatedDomainEvent(
        Guid paymentId,
        Guid serviceRequestId,
        decimal amount)
    {
        PaymentId = paymentId;
        ServiceRequestId = serviceRequestId;
        Amount = amount;
    }
}