using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Entities;

public class Review : Entity
{
    private Review()
    {
    }

    public Guid CustomerId { get; private set; }

    public Guid ArtisanId { get; private set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; } = string.Empty;

    public Review(
        Guid customerId,
        Guid artisanId,
        int rating,
        string comment)
    {
        CustomerId = customerId;
        ArtisanId = artisanId;
        Rating = rating;
        Comment = comment;
    }
}