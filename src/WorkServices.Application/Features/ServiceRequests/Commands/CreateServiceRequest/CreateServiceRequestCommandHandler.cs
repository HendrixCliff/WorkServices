using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces;
using WorkServices.Domain.Entities;

namespace WorkServices.Application.Features.ServiceRequests.Commands.CreateServiceRequest;


public class CreateServiceRequestCommandHandler
    : IRequestHandler<CreateServiceRequestCommand, Guid>
{
    private readonly IServiceRequestRepository _requests;
    private readonly IUnitOfWork _uow;

    public CreateServiceRequestCommandHandler(
        IServiceRequestRepository requests,
        IUnitOfWork uow)
    {
        _requests = requests;
        _uow = uow;
    }

    public async Task<Guid> Handle(
        CreateServiceRequestCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest =
            new ServiceRequest(
                request.CustomerId,
                request.ServiceType,
                request.Description,
                request.Address);

        await _requests.AddAsync(serviceRequest);

        await _uow.SaveChangesAsync(
            cancellationToken);

        return serviceRequest.Id;
    }
}