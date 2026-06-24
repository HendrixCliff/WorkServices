using MediatR;

namespace WorkServices.Application.Features.JobAssignments.Commands.RejectJob;

public sealed record RejectJobCommand(
    Guid AssignmentId)
    : IRequest;