using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Domain.Abstractions;
using WorkServices.Infrastructure.Persistence;

namespace WorkServices.Infrastructure.DomainEvents;

public sealed class DomainEventDispatcher
    : IDomainEventDispatcher
{
    private readonly ApplicationDbContext _db;
    private readonly IMediator _mediator;

    public DomainEventDispatcher(
        ApplicationDbContext db,
        IMediator mediator)
    {
        _db = db;
        _mediator = mediator;
    }

    public async Task DispatchAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = _db.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        entities.ForEach(x => x.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(
                domainEvent,
                cancellationToken);
        }
    }
}