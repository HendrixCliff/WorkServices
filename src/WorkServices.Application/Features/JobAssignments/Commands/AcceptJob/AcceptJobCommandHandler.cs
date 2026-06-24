using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces;

namespace WorkServices.Application.Features.JobAssignments.Commands.AcceptJob;

public sealed class AcceptJobCommandHandler
    : IRequestHandler<AcceptJobCommand>
{
    private readonly IJobAssignmentRepository _repository;

    private readonly IUnitOfWork _unitOfWork;

    public AcceptJobCommandHandler(
        IJobAssignmentRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AcceptJobCommand request,
        CancellationToken cancellationToken)
    {
        var assignment =
            await _repository.GetByIdAsync(
                request.AssignmentId);

        if (assignment is null)
            throw new Exception("Assignment not found");

        assignment.Accept();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}