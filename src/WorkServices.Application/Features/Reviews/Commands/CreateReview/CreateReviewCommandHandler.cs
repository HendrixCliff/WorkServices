using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;
using WorkServices.Domain.Enums;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IReviewRepository _reviewRepository;

    private readonly IArtisanRepository _artisanRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IServiceRequestRepository _serviceRequests;
  
   public CreateReviewCommandHandler(
    IReviewRepository reviewRepository,
    IArtisanRepository artisanRepository,
    IServiceRequestRepository serviceRequests,
    IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _artisanRepository = artisanRepository;
        _serviceRequests = serviceRequests;
        _unitOfWork = unitOfWork;
    }

   public async Task<Guid> Handle(
    CreateReviewCommand request,
    CancellationToken cancellationToken)
{
    var serviceRequest =
        await _serviceRequests.GetByIdAsync(
            request.ServiceRequestId);

    if (serviceRequest is null)
        throw new NotFoundException(
            "Service request not found");

    if (serviceRequest.Status != JobStatus.LabourPaid)
       throw new NotFoundException(
            "Cannot review until payment is complete.");

    var artisan =
        await _artisanRepository.GetByIdAsync(
            request.ArtisanId);

    if (artisan is null)
        throw new NotFoundException(
            "Artisan not found");

    var review =
        new Review(
        request.ServiceRequestId,
        request.CustomerId,
        request.ArtisanId,
        request.Rating,
        request.Comment);

    artisan.UpdateRating(request.Rating);

    serviceRequest.MarkReviewed();

    await _reviewRepository.AddAsync(review);

    await _unitOfWork.SaveChangesAsync(
        cancellationToken);

    return review.Id;
}
}