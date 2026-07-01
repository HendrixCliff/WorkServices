using WorkServices.Infrastructure.Persistence;

namespace WorkServices.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        ApplicationDbContext db,
        CancellationToken cancellationToken = default);
}