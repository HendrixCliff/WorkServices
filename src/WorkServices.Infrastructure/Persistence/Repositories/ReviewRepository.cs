using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public sealed class ReviewRepository
    : IReviewRepository
{
    private readonly ApplicationDbContext _db;

    public ReviewRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        Review review)
    {
        await _db.Reviews.AddAsync(
            review);
    }

    public async Task<Review?> GetByIdAsync(
        Guid id)
    {
        return await _db.Reviews
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task<List<Review>>
        GetArtisanReviewsAsync(
            Guid artisanId)
    {
        return await _db.Reviews
            .Where(x =>
                x.ArtisanId ==
                artisanId)
            .OrderByDescending(
                x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Review>> GetByArtisanIdAsync( Guid artisanId)
        {
            return await _db.Reviews
                .Where(x => x.ArtisanId == artisanId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
}