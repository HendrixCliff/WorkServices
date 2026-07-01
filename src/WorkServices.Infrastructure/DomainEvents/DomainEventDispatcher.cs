using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Domain.Abstractions;
using WorkServices.Infrastructure.Persistence;

namespace WorkServices.Infrastructure.DomainEvents;

public sealed class DomainEventDispatcher
    : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync(
        ApplicationDbContext db,
        CancellationToken cancellationToken = default)
    {
        var entities = db.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var events = entities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        entities.ForEach(x => x.ClearDomainEvents());

        foreach (var domainEvent in events)
        {
            await _mediator.Publish(
                domainEvent,
                cancellationToken);
        }
    }
}