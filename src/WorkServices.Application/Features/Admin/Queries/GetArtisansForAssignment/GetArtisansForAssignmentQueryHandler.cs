
using MediatR;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Application.Features.Admin.Queries.GetArtisansForAssignment;

public sealed class GetArtisansForAssignmentQueryHandler
    : IRequestHandler<
        GetArtisansForAssignmentQuery,
        IReadOnlyList<ArtisanForAssignmentDto>>
{
    private readonly IArtisanRepository _artisanRepository;

    public GetArtisansForAssignmentQueryHandler(
        IArtisanRepository artisanRepository)
    {
        _artisanRepository = artisanRepository;
    }

    public async Task<IReadOnlyList<ArtisanForAssignmentDto>> Handle(
        GetArtisansForAssignmentQuery request,
        CancellationToken cancellationToken)
    {
        var artisans = await _artisanRepository.GetAvailableAsync();

        return artisans
            .OrderBy(artisan => artisan.FullName)
            .Select(artisan => new ArtisanForAssignmentDto
            {
                Id = artisan.Id,
                FullName = artisan.FullName,
                Email = artisan.Email,
                ServiceType = artisan.ServiceType.ToString()
            })
            .ToList();
    }
}
