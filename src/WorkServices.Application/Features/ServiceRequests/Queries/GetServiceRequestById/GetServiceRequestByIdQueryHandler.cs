using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.DTOs.ServiceRequests;
using System.Linq;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetServiceRequestById;

public sealed class GetServiceRequestByIdQueryHandler
    : IRequestHandler<
        GetServiceRequestByIdQuery,
        ServiceRequestDetailsDto>
{
    private readonly IServiceRequestRepository _repository;

    public GetServiceRequestByIdQueryHandler(
        IServiceRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceRequestDetailsDto> Handle(
        GetServiceRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sr = await _repository.GetByIdAsync(request.ServiceRequestId);

    if (sr is null)
        throw new Exception("Service request not found");
        
       var assignment = sr.JobAssignments.FirstOrDefault();

    return new ServiceRequestDetailsDto
    {
        Id = sr.Id,
        Customer = sr.Customer?.FullName ?? "",
        Address = sr.Address,
        Description = sr.Description,
        ServiceType = sr.ServiceType.ToString(),
        Status = sr.Status.ToString(),

        AssignedArtisanId = assignment?.ArtisanId,

       AssignedArtisan = string.Empty
};
}
}