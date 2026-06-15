using Microsoft.AspNetCore.SignalR;
using WorkServices.API.Hubs;
using WorkServices.Application.Interfaces;

namespace WorkServices.Infrastructure.Notifications;

public class SignalRNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotifier(
        IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public async Task NotifyArtisanAsync(
        Guid artisanId,
        object payload)
    {
        await _hub.Clients
            .Group($"artisan-{artisanId}")
            .SendAsync("JobAssigned", payload);
    }

    public async Task NotifyCustomerAsync(
        Guid customerId,
        object payload)
    {
        await _hub.Clients
            .Group($"customer-{customerId}")
            .SendAsync("Notification", payload);
    }

    public async Task NotifyAdminAsync(
        string message)
    {
        await _hub.Clients
            .Group("admins")
            .SendAsync("Notification", message);
    }
}