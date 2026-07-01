using MediatR;
using WorkServices.Application.Common;
using WorkServices.Application.DTOs.Reviews;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Common.Pagination; 

namespace WorkServices.Application.Features.Reviews.Queries.GetReviewsByArtisan;

public sealed class GetReviewsByArtisanQueryHandler
    : IRequestHandler<
        GetReviewsByArtisanQuery,
        PagedResult<ReviewDto>>
{
    private readonly IReviewRepository _repository;

    public GetReviewsByArtisanQueryHandler(
        IReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<ReviewDto>> Handle(
        GetReviewsByArtisanQuery request,
        CancellationToken cancellationToken)
    {
        var reviews =
            await _repository.GetByArtisanIdAsync(
                request.ArtisanId);

        var ordered = reviews
            .OrderByDescending(x => x.CreatedAt);

        var total = ordered.Count();

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ReviewDto
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                ArtisanId = x.ArtisanId,
                ServiceRequestId = x.ServiceRequestId,
                Rating = x.Rating,
                Comment = x.Comment,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        return new PagedResult<ReviewDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = total
        };
    }
}