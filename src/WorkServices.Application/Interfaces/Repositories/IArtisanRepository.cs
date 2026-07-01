using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Repositories;

public interface IArtisanRepository
{
    Task<Artisan?> GetByIdAsync(Guid id);

    Task<List<Artisan>> GetAvailableAsync();

    Task AddAsync(Artisan artisan);

    void Update(Artisan artisan);
}