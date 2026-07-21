using MediatR;
using WorkServices.Application.DTOs.Admin;
using WorkServices.Application.Interfaces.Repositories;


namespace WorkServices.Application.Features.ServiceRequests.Queries.GetServiceRequestsForAssignment;

public sealed record GetServiceRequestsForAssignmentQuery()
    : IRequest<List<ServiceRequestForAssignmentDto>>;