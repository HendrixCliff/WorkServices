namespace WorkServices.Application.DTOs.Reviews;

public sealed class ReviewDto
{
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    public Guid ArtisanId { get; init; }

    public Guid ServiceRequestId { get; init; }

    public int Rating { get; init; }

    public string Comment { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}