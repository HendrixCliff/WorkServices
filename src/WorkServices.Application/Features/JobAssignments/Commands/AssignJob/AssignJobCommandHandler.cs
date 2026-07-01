using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Application.Features.JobAssignments.Commands.AssignJob;

public sealed class AssignJobCommandHandler
    : IRequestHandler<AssignJobCommand, Guid>
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

    public async Task<Guid> Handle(
        AssignJobCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest =
            await _serviceRequestRepository.GetByIdAsync(
                request.ServiceRequestId)
            ?? throw new Exception("Service request not found");

        var artisan =
            await _artisanRepository.GetByIdAsync(
                request.ArtisanId)
            ?? throw new Exception("Artisan not found");

        var assignment =
            new JobAssignment(
                serviceRequest.Id,
                artisan.Id);

       serviceRequest.Assign(artisan.Id);

        await _assignmentRepository.AddAsync(
            assignment);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return assignment.Id;
    }
}