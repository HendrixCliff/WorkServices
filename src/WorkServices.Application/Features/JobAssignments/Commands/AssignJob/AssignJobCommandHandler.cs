
using MediatR;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Application.Features.JobAssignments.Commands.AssignJob;

public sealed class AssignJobCommandHandler
    : IRequestHandler<AssignJobCommand, Unit>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IArtisanRepository _artisanRepository;
    private readonly IJobAssignmentRepository _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignJobCommandHandler(
        IServiceRequestRepository serviceRequestRepository,
        IArtisanRepository artisanRepository,
        IJobAssignmentRepository assignmentRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _artisanRepository = artisanRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        AssignJobCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest =
            await _serviceRequestRepository.GetByIdAsync(
                request.ServiceRequestId)
            ?? throw new NotFoundException(
                "Service request not found.");

        var availableArtisans =
            await _artisanRepository.GetAvailableAsync();

        var artisan = availableArtisans.FirstOrDefault(
            x => x.Id == request.ArtisanId)
            ?? throw new NotFoundException(
                "Artisan not found or is no longer available.");

        var assignment = new JobAssignment(
            serviceRequest.Id,
            artisan.Id);

        serviceRequest.Assign(artisan.Id);

        artisan.SetAvailability(false);

        await _assignmentRepository.AddAsync(assignment);

        _artisanRepository.Update(artisan);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
