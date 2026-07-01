using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Events;

public sealed class ReviewSubmittedDomainEvent
    : DomainEvent
{
    public Guid ServiceRequestId { get; }

    public Guid ArtisanId { get; }

    public Guid CustomerId { get; }

    public int Rating { get; }

    public ReviewSubmittedDomainEvent(
        Guid serviceRequestId,
        Guid artisanId,
        Guid customerId,
        int rating)
    {
        ServiceRequestId = serviceRequestId;
        ArtisanId = artisanId;
        CustomerId = customerId;
        Rating = rating;
    }
}