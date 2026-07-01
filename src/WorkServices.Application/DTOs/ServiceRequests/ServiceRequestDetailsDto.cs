namespace WorkServices.Application.DTOs.ServiceRequests;

public sealed class ServiceRequestDetailsDto
{
    public Guid Id { get; set; }

    public string Customer { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ServiceType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public Guid? AssignedArtisanId { get; set; }

    public string AssignedArtisan { get; set; } = string.Empty;
}