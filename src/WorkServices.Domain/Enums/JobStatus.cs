namespace WorkServices.Domain.Enums;

public enum JobStatus
{
    Pending = 1,

    Assigned = 2,

    QuoteSubmitted = 3,

    QuoteApproved = 4,

    MaterialsPaid = 5,

    InProgress = 6,

    Completed = 7,

    LabourPaid = 8,

    Reviewed = 9,

    Cancelled = 10
}