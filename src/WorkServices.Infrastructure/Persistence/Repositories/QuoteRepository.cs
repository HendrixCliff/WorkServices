using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public sealed class QuoteRepository
    : IQuoteRepository
{
    private readonly ApplicationDbContext _db;

    public QuoteRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        Quote quote)
    {
        await _db.Set<Quote>()
            .AddAsync(quote);
    }

    public async Task<Quote?> GetByIdAsync(
        Guid id)
    {
        return await _db.Set<Quote>()
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task<Quote?> GetByServiceRequestIdAsync(
        Guid serviceRequestId)
    {
        return await _db.Set<Quote>()
            .FirstOrDefaultAsync(
                x => x.ServiceRequestId == serviceRequestId);
    }
        public void Update(
        Quote quote)
    {
        _db.Set<Quote>()
            .Update(quote);
    }
}