using MediatR;

namespace WorkServices.Application.Features.Reviews.Commands.CreateReview;

public sealed record CreateReviewCommand(
    Guid ServiceRequestId,
    Guid CustomerId,
    Guid ArtisanId,
    int Rating,
    string Comment)
    : IRequest<Guid>;