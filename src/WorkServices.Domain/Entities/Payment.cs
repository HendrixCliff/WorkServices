
using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Enums;
using WorkServices.Domain.Events;

namespace WorkServices.Domain.Entities;

public class Payment : Entity
{
    private Payment()
    {
    }

    public Guid ServiceRequestId { get; private set; }

    public decimal Amount { get; private set; }

    public PaymentType Type { get; private set; }

    public string Reference { get; private set; } = string.Empty;

    public PaymentStatus Status { get; private set; }

    public ServiceRequest ServiceRequest { get; private set; } = null!;

    public Payment(
        Guid serviceRequestId,
        decimal amount,
        PaymentType paymentType)
    {
        if (serviceRequestId == Guid.Empty)
        {
            throw new ArgumentException(
                "Service request ID is required.",
                nameof(serviceRequestId));
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Payment amount must be greater than zero.");
        }

        if (!Enum.IsDefined(typeof(PaymentType), paymentType))
        {
            throw new ArgumentOutOfRangeException(
                nameof(paymentType),
                "Invalid payment type.");
        }

        ServiceRequestId = serviceRequestId;
        Amount = amount;
        Type = paymentType;
        Status = PaymentStatus.Pending;

        switch (paymentType)
        {
            case PaymentType.Material:
                AddDomainEvent(
                    new MaterialPaymentCreatedDomainEvent(
                        Id,
                        ServiceRequestId,
                        Amount));
                break;

            case PaymentType.Labour:
                AddDomainEvent(
                    new LabourPaymentCreatedDomainEvent(
                        Id,
                        ServiceRequestId,
                        Amount));
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(paymentType),
                    "Unsupported payment type.");
        }
    }

    public void SetReference(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            throw new ArgumentException(
                "Payment reference is required.",
                nameof(reference));
        }

        if (IsPaid())
        {
            throw new InvalidOperationException(
                "A successful payment reference cannot be changed.");
        }

        if (!string.IsNullOrWhiteSpace(Reference) &&
            !string.Equals(
                Reference,
                reference,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "This payment already has a different reference.");
        }

        Reference = reference;
        MarkUpdated();
    }

    public void ConfirmMaterialPayment(string reference)
    {
        EnsureReferenceMatches(reference);

        if (Type != PaymentType.Material)
        {
            throw new InvalidOperationException(
                "This is not a materials payment.");
        }

        if (Status == PaymentStatus.MaterialPaid)
        {
            return; // Idempotent duplicate confirmation.
        }

        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only a pending materials payment can be confirmed.");
        }

        Status = PaymentStatus.MaterialPaid;

        AddDomainEvent(
            new MaterialsPaidDomainEvent(
                ServiceRequestId,
                Id));

        MarkUpdated();
    }

    public void ConfirmLabourPayment(string reference)
    {
        EnsureReferenceMatches(reference);

        if (Type != PaymentType.Labour)
        {
            throw new InvalidOperationException(
                "This is not a labour payment.");
        }

        if (Status == PaymentStatus.LabourPaid)
        {
            return; // Idempotent duplicate confirmation.
        }

        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidOperationException(
                "Only a pending labour payment can be confirmed.");
        }

        Status = PaymentStatus.LabourPaid;

        AddDomainEvent(
            new LabourPaidDomainEvent(
                ServiceRequestId,
                Id));

        MarkUpdated();
    }

    public void MarkFailed()
    {
        if (IsPaid())
        {
            throw new InvalidOperationException(
                "A successful payment cannot be marked as failed.");
        }

        if (Status == PaymentStatus.Failed)
        {
            return;
        }

        Status = PaymentStatus.Failed;
        MarkUpdated();
    }

    private void EnsureReferenceMatches(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            throw new ArgumentException(
                "Payment reference is required.",
                nameof(reference));
        }

        if (string.IsNullOrWhiteSpace(Reference) ||
            !string.Equals(
                Reference,
                reference,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "The verified reference does not match this payment.");
        }
    }

    private bool IsPaid() =>
        Status is PaymentStatus.MaterialPaid
            or PaymentStatus.LabourPaid;
}
