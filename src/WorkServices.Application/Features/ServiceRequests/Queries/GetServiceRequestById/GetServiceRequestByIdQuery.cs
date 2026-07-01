using MediatR;
using WorkServices.Application.DTOs.ServiceRequests;

namespace WorkServices.Application.Features.ServiceRequests.Queries.GetServiceRequestById;

public sealed record GetServiceRequestByIdQuery(
    Guid ServiceRequestId)
    : IRequest<ServiceRequestDetailsDto>;