namespace WorkServices.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; protected set; }

    protected void MarkUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}