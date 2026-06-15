namespace WorkServices.Application.DTOs.Assignments;

public sealed class AssignmentNotificationDto
{
    public Guid AssignmentId { get; set; }

    public Guid ServiceRequestId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string ServiceType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}