using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public sealed class ArtisanRepository
    : IArtisanRepository
{
    private readonly ApplicationDbContext _db;

    public ArtisanRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Artisan?> GetByIdAsync(
        Guid id)
    {
        return await _db.Artisans
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task<List<Artisan>>
        GetAvailableAsync()
    {
        return await _db.Artisans
            .Where(x => x.IsAvailable)
            .ToListAsync();
    }

    public async Task AddAsync(
        Artisan artisan)
    {
        await _db.Artisans.AddAsync(
            artisan);
    }

    public void Update(
        Artisan artisan)
    {
        _db.Artisans.Update(
            artisan);
    }
}