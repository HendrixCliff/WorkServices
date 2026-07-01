using MediatR;

namespace WorkServices.Application.Features.JobAssignments.Commands.StartJob;

public sealed record StartJobCommand(
    Guid ServiceRequestId,
    Guid ArtisanId)
    : IRequest;