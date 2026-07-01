using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Entities;

public sealed class Quote : Entity
{
    private Quote()
    {
    }

    public Guid ServiceRequestId { get; private set; }

    public Guid ArtisanId { get; private set; }

    public decimal MaterialCost { get; private set; }

    public decimal LabourCost { get; private set; }

    public decimal TotalCost =>
        MaterialCost + LabourCost;

    public string Notes { get; private set; } = string.Empty;

    public bool Approved { get; private set; }

    public Quote(
        Guid serviceRequestId,
        Guid artisanId,
        decimal materialCost,
        decimal labourCost,
        string notes)
    {
        ServiceRequestId = serviceRequestId;
        ArtisanId = artisanId;
        MaterialCost = materialCost;
        LabourCost = labourCost;
        Notes = notes;
    }

    public void Approve()
    {
        Approved = true;
        MarkUpdated();
    }
}