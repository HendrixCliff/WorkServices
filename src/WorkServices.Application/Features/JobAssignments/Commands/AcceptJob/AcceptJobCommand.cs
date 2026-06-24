using MediatR;

namespace WorkServices.Application.Features.JobAssignments.Commands.AcceptJob;

public sealed record AcceptJobCommand(
    Guid AssignmentId)
    : IRequest;