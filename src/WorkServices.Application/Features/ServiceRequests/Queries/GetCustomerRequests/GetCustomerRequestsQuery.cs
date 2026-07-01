using MediatR;
using WorkServices.Application.DTOs.Customers;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetCustomerRequests;

public sealed record GetCustomerRequestsQuery(
    Guid CustomerId)
    : IRequest<List<CustomerRequestDto>>;