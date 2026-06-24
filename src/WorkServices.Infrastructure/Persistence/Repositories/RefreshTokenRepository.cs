using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository
    : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _db;

    public RefreshTokenRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        RefreshToken token)
    {
        await _db.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> GetByTokenAsync(
        string token)
    {
        return await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public Task UpdateAsync(
        RefreshToken token)
    {
        _db.RefreshTokens.Update(token);

        return Task.CompletedTask;
    }
}