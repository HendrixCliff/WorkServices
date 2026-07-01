using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Events;

namespace WorkServices.Domain.Entities;

public class Review : Entity
{
    private Review()
    {
    }

    public Guid ServiceRequestId { get; private set; }

    public Guid CustomerId { get; private set; }

    public Guid ArtisanId { get; private set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; } = string.Empty;

    public Review(
        Guid serviceRequestId,
        Guid customerId,
        Guid artisanId,
        int rating,
        string comment)
    {
        ServiceRequestId = serviceRequestId;
        CustomerId = customerId;
        ArtisanId = artisanId;
        Rating = rating;
        Comment = comment;

        AddDomainEvent(
            new ReviewSubmittedDomainEvent(
                serviceRequestId,
                artisanId,
                customerId,
                rating));
    }
}