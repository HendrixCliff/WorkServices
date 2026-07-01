using MediatR;

namespace WorkServices.Application.Features.JobAssignments.Commands.CompleteJob;

public sealed record CompleteJobCommand(
    Guid ServiceRequestId,
    Guid ArtisanId)
    : IRequest;