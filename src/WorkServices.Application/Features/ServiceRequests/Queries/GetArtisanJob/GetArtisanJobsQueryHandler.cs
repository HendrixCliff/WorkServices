using MediatR;
using WorkServices.Application.DTOs.Artisans;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetArtisanJob;

public sealed class GetArtisanJobsQueryHandler
    : IRequestHandler<GetArtisanJobsQuery, List<ArtisanJobDto>>
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
        var assignments = await _repository.GetArtisanAssignmentsAsync(
            request.ArtisanId);

        return assignments
            .Where(assignment => assignment.ServiceRequest is not null)
            .Select(assignment =>
            {
                var serviceRequest = assignment.ServiceRequest!;

                return new ArtisanJobDto
                {
                    AssignmentId = assignment.Id,
                    ServiceRequestId = serviceRequest.Id,
                    Address = serviceRequest.Address,
                    Description = serviceRequest.Description,
                    Accepted = assignment.Status == AssignmentStatus.Accepted,
                    CustomerName = serviceRequest.Customer?.FullName
                        ?? string.Empty,
                    ServiceType = serviceRequest.ServiceType.ToString(),
                    Status = serviceRequest.Status.ToString(),
                    EstimatedCost = serviceRequest.EstimatedCost
                };
            })
            .ToList();
    }
}