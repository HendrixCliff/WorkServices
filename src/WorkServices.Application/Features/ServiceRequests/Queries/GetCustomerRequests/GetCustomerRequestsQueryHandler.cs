using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.DTOs.Customers;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetCustomerRequests;

public sealed class GetCustomerRequestsQueryHandler
    : IRequestHandler<
        GetCustomerRequestsQuery,
        List<CustomerRequestDto>>
{
    private readonly IServiceRequestRepository _repository;

    public GetCustomerRequestsQueryHandler(
        IServiceRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CustomerRequestDto>>
        Handle(
            GetCustomerRequestsQuery request,
            CancellationToken cancellationToken)
    {
        var requests =
            await _repository
                .GetCustomerRequestsAsync(
                    request.CustomerId);

        return requests
            .Select(x => new CustomerRequestDto
            {
                Id = x.Id,
                Address = x.Address,
                ServiceType =
                    x.ServiceType.ToString(),
                Status =
                    x.Status.ToString()
            })
            .ToList();
    }
}