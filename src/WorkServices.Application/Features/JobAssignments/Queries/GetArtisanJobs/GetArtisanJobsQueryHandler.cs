using MediatR;
using WorkServices.Application.DTOs.Artisans;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Application.Features.JobAssignments.Queries.GetArtisanJobs;

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
        var jobs =
            await _repository.GetArtisanJobsAsync(
                request.ArtisanId);

        return jobs
            .Select(x => new ArtisanJobDto
            {
                AssignmentId = x.Id,

                ServiceRequestId = x.ServiceRequestId,

                CustomerName =
                    x.ServiceRequest.Customer!.FullName,

                Address =
                    x.ServiceRequest.Address,

                ServiceType =
                    x.ServiceRequest.ServiceType.ToString(),

                Status =
                    x.ServiceRequest.Status.ToString(),

                EstimatedCost =
                    x.ServiceRequest.EstimatedCost
            })
            .ToList();
    }
}