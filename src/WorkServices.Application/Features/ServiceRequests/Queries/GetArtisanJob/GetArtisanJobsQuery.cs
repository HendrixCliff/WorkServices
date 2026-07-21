using WorkServices.Application.DTOs.Artisans;
using MediatR;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetArtisanJob;

public sealed record GetArtisanJobsQuery(
    Guid ArtisanId)
    : IRequest<List<ArtisanJobDto>>;