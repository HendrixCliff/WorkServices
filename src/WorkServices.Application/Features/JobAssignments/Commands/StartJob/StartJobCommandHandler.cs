using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.JobAssignments.Commands.StartJob;

public sealed class StartJobCommandHandler
    : IRequestHandler<StartJobCommand>
{
    private readonly IServiceRequestRepository _serviceRequests;
    private readonly IUnitOfWork _unitOfWork;

    public StartJobCommandHandler(
        IServiceRequestRepository serviceRequests,
        IUnitOfWork unitOfWork)
    {
        _serviceRequests = serviceRequests;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        StartJobCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest =
            await _serviceRequests.GetByIdAsync(request.ServiceRequestId)
            ?? throw new NotFoundException("Service request not found.");

       serviceRequest.Start();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}