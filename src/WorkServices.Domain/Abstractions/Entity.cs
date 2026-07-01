namespace WorkServices.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<DomainEvent> _domainEvents = [];

    public Guid Id { get; protected set; }
        = Guid.NewGuid();

    public DateTime CreatedAt { get; protected set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; protected set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(
        DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}