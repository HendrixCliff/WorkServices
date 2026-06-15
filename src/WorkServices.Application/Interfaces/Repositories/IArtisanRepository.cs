namespace WorkServices.Application.Interfaces.Repositories;

public interface IArtisanRepository
{
    Task<Artisan?> GetByIdAsync(Guid id);
}