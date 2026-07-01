namespace WorkServices.Domain.Enums;

public enum PaymentStatus
{
    Pending = 1,

    MaterialPaid = 2,

    LabourPaid = 3,

    Failed = 4,

    Refunded = 5
}