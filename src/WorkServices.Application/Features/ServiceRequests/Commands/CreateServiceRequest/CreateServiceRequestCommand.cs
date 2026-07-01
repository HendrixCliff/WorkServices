using MediatR;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.ServiceRequests.Commands.CreateServiceRequest;

public sealed record CreateServiceRequestCommand(
    Guid CustomerId,
    ServiceType ServiceType,
    string Description,
    string Address)
    : IRequest<Guid>;