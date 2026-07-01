namespace WorkServices.Application.DTOs.Notification;

public sealed class NotificationDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public bool IsRead { get; init; }

    public DateTime CreatedAt { get; init; }
}