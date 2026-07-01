using WorkServices.Application.DTOs.ArtisanJob;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Enums;
using MediatR;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetArtisanJob;

public sealed class GetArtisanJobsQueryHandler
    : IRequestHandler<
        GetArtisanJobsQuery,
        List<ArtisanJobDto>>
{
    private readonly IJobAssignmentRepository _repository;

    public GetArtisanJobsQueryHandler(
        IJobAssignmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ArtisanJobDto>> Handle(
            GetArtisanJobsQuery request,
            CancellationToken cancellationToken)
    {
        var assignments =
            await _repository
                .GetArtisanAssignmentsAsync(
                    request.ArtisanId);

        return assignments
            .Select(x => new ArtisanJobDto
            {
                AssignmentId = x.Id,
                Accepted = x.Status == AssignmentStatus.Accepted,
                Address =
                    x.ServiceRequest?.Address ?? "",
                Description =
                    x.ServiceRequest?.Description ?? ""
            })
            .ToList();
    }
}