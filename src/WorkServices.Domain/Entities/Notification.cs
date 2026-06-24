using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Entities;

public sealed class Notification : Entity
{
    private Notification()
    {
    }

    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public bool IsRead { get; private set; }

    public Notification(
        Guid userId,
        string title,
        string message)
    {
        UserId = userId;
        Title = title;
        Message = message;

        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        MarkUpdated();
    }
}