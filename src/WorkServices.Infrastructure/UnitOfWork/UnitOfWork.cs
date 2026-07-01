using WorkServices.Application.Interfaces;
using WorkServices.Infrastructure.Persistence;

namespace WorkServices.Infrastructure.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    private readonly IDomainEventDispatcher _dispatcher;

    public UnitOfWork(
        ApplicationDbContext db,
        IDomainEventDispatcher dispatcher)
    {
        _db = db;
        _dispatcher = dispatcher;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var result =
            await _db.SaveChangesAsync(cancellationToken);

        await _dispatcher.DispatchAsync(
            cancellationToken);

        return result;
    }
}