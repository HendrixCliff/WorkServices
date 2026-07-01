namespace WorkServices.Application.DTOs.Quotes;

public sealed class QuoteDto
{
    public Guid Id { get; init; }

    public Guid ServiceRequestId { get; init; }

    public decimal LabourCost { get; init; }

    public decimal MaterialCost { get; init; }

    public decimal TotalCost { get; init; }

    public bool Approved { get; init; }
}