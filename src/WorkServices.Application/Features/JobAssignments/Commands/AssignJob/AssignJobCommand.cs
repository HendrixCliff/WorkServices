using MediatR;

namespace WorkServices.Application.Features.JobAssignments.Commands.AssignJob;

public sealed record AssignJobCommand(
    Guid ServiceRequestId,
    Guid ArtisanId
) : IRequest<Unit>;