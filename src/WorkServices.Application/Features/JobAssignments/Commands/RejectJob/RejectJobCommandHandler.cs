using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.JobAssignments.Commands.RejectJob;

public sealed class RejectJobCommandHandler
    : IRequestHandler<RejectJobCommand>
{
    private readonly IJobAssignmentRepository _repository;

    private readonly IUnitOfWork _unitOfWork;

    public RejectJobCommandHandler(
        IJobAssignmentRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        RejectJobCommand request,
        CancellationToken cancellationToken)
    {
        var assignment =
            await _repository.GetByIdAsync(
                request.AssignmentId);

        if (assignment is null)
            throw new NotFoundException("Assignment not found");

        assignment.Reject();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}