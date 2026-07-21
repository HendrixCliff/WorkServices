namespace WorkServices.Application.DTOs.Admin;

public sealed class ServiceRequestForAssignmentDto
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string ServiceType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal EstimatedCost { get; set; }

    public DateTime CreatedAt { get; set; }
}