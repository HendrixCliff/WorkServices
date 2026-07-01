using MediatR;
using WorkServices.Application.DTOs.Reviews;
using WorkServices.Application.Common.Pagination;

namespace WorkServices.Application.Features.Reviews.Queries.GetReviewsByArtisan;

public sealed record GetReviewsByArtisanQuery(
    Guid ArtisanId,
     int Page = 1,
    int PageSize = 20)
    : IRequest<PagedResult<ReviewDto>>;