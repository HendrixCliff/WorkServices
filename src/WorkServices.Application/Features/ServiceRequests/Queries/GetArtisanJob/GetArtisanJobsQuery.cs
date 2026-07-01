using WorkServices.Application.DTOs.ArtisanJob;
using MediatR;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetArtisanJob;

public sealed record GetArtisanJobsQuery(
    Guid ArtisanId)
    : IRequest<List<ArtisanJobDto>>;