namespace WorkServices.Application.Features.Admin.Queries.GetArtisansForAssignment;

public sealed class ArtisanForAssignmentDto
{
    public Guid Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string ServiceType { get; init; } = string.Empty;
}