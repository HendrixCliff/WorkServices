using MediatR;
using WorkServices.Application.DTOs.Artisans;

namespace WorkServices.Application.Features.JobAssignments.Queries.GetArtisanJobs;

public sealed record GetArtisanJobsQuery(
    Guid ArtisanId)
    : IRequest<List<ArtisanJobDto>>;