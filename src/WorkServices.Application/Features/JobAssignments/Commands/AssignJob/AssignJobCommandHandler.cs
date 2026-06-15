using MediatR;
using WorkServices.Application.Events;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Application.Features.JobAssignments.Commands.AssignJob;

public sealed class AssignJobCommandHandler
    : IRequestHandler<AssignJobCommand, Guid>
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IArtisanRepository _artisanRepository;
    private readonly IJobAssignmentRepository _assignmentRepository;
    private readonly IMediator _mediator;

    public AssignJobCommandHandler(
        IServiceRequestRepository serviceRequestRepository,
        IArtisanRepository artisanRepository,
        IJobAssignmentRepository assignmentRepository,
        IMediator mediator)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _artisanRepository = artisanRepository;
        _assignmentRepository = assignmentRepository;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(
        AssignJobCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest =
            await _serviceRequestRepository.GetByIdAsync(
                request.ServiceRequestId);

        if (serviceRequest is null)
            throw new Exception("Service request not found");

        var artisan =
            await _artisanRepository.GetByIdAsync(
                request.ArtisanId);

        if (artisan is null)
            throw new Exception("Artisan not found");

        var assignment = new JobAssignment(
            serviceRequest.Id,
            artisan.Id);

        serviceRequest.Assign();

        await _assignmentRepository.AddAsync(
            assignment);

        await _mediator.Publish(
            new JobAssignedEvent(
                assignment.Id,
                artisan.Id,
                serviceRequest.CustomerId,
                serviceRequest.Customer?.FullName ?? string.Empty,
                serviceRequest.Customer?.PhoneNumber ?? string.Empty,
                serviceRequest.Address,
                serviceRequest.ServiceType.ToString(),
                serviceRequest.Description),
            cancellationToken);

        return assignment.Id;
    }
}