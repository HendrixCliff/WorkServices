using MediatR;
using WorkServices.Application.DTOs.Customers;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetMyRequests;

public sealed class GetMyRequestsQueryHandler
    : IRequestHandler<
        GetMyRequestsQuery,
        List<CustomerRequestDto>>
{
    private readonly IServiceRequestRepository _repository;

    public GetMyRequestsQueryHandler(
        IServiceRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CustomerRequestDto>> Handle(
        GetMyRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var requests =
            await _repository.GetCustomerRequestsAsync(
                request.CustomerId);

        return requests
            .Select(x => new CustomerRequestDto
            {
                Id = x.Id,
                Address = x.Address,
                ServiceType = x.ServiceType.ToString(),
                Status = x.Status.ToString()
            })
            .ToList();
    }
}