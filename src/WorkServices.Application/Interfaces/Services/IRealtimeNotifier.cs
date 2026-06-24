namespace WorkServices.Application.Interfaces.Services;

public interface IRealtimeNotifier
{
    Task NotifyUserAsync(
        Guid userId,
        string title,
        string message);
}