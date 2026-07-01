using MediatR;

namespace WorkServices.Domain.Abstractions;

public abstract class DomainEvent
    : INotification
{
    public DateTime OccurredOn { get; }
        = DateTime.UtcNow;
}