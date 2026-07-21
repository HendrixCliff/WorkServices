using MediatR;
using WorkServices.Application.DTOs.Customers;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetMyRequests;

public sealed record GetMyRequestsQuery(
    Guid CustomerId)
    : IRequest<List<CustomerRequestDto>>;