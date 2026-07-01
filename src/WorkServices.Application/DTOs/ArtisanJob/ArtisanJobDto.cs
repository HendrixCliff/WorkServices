namespace WorkServices.Application.DTOs.ArtisanJob;

public sealed class ArtisanJobDto
{
    public Guid AssignmentId { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool Accepted { get; set; }
}