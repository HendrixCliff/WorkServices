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
        }
    }

    public void ConfirmMaterialPayment(string reference)
{
    Reference = reference;

    Status = PaymentStatus.MaterialPaid;

    AddDomainEvent(
        new MaterialsPaidDomainEvent(
            ServiceRequestId,
            Id));

    MarkUpdated();
}

   public void ConfirmLabourPayment(string reference)
    {
        Reference = reference;

        Status = PaymentStatus.LabourPaid;

        AddDomainEvent(
            new LabourPaidDomainEvent(
                ServiceRequestId,
                Id));

        MarkUpdated();
    }

    public void SetReference(string reference)
    {
        Reference = reference;
    }

    public void MarkFailed()
    {
        Status = PaymentStatus.Failed;

        MarkUpdated();
    }
}