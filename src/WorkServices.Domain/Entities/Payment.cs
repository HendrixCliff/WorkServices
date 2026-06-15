using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public class Payment : Entity
{
    private Payment()
    {
    }

    public Guid ServiceRequestId { get; private set; }

    public decimal Amount { get; private set; }

    public string Reference { get; private set; } = string.Empty;

    public PaymentStatus Status { get; private set; }

    public Payment(
        Guid requestId,
        decimal amount)
    {
        ServiceRequestId = requestId;
        Amount = amount;

        Status = PaymentStatus.Pending;
    }

    public void Confirm(string reference)
    {
        Reference = reference;
        Status = PaymentStatus.Completed;
        MarkUpdated();
    }
}