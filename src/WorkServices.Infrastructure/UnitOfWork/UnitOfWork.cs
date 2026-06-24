using WorkServices.Application.Interfaces;
using WorkServices.Infrastructure.Persistence;

namespace WorkServices.Infrastructure.UnitOfWork;

public sealed class UnitOfWork
    : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(
            cancellationToken);
    }
}