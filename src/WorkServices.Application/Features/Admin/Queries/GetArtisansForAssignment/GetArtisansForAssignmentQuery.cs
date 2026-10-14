
using MediatR;

namespace WorkServices.Application.Features.Admin.Queries.GetArtisansForAssignment;

public sealed record GetArtisansForAssignmentQuery
    : IRequest<IReadOnlyList<ArtisanForAssignmentDto>>;
