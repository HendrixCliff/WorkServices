namespace WorkServices.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        CancellationToken cancellationToken = default);
}