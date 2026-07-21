using MediatR;
using WorkServices.Application.DTOs.Admin;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetServiceRequestsForAssignment;

public sealed class GetServiceRequestsForAssignmentQueryHandler
    : IRequestHandler<
        GetServiceRequestsForAssignmentQuery,
        List<ServiceRequestForAssignmentDto>>
{
    private readonly IServiceRequestRepository _repository;

    public GetServiceRequestsForAssignmentQueryHandler(
        IServiceRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ServiceRequestForAssignmentDto>> Handle(
        GetServiceRequestsForAssignmentQuery request,
        CancellationToken cancellationToken)
    {
        var requests =
            await _repository.GetRequestsForAssignmentAsync();

        return requests
            .Select(x => new ServiceRequestForAssignmentDto
            {
                Id = x.Id,
                CustomerName = x.Customer!.FullName,
                Address = x.Address,
                ServiceType = x.ServiceType.ToString(),
                Status = x.Status.ToString(),
                EstimatedCost = x.EstimatedCost
            })
            .ToList();
    }
}