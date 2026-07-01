using Microsoft.AspNetCore.SignalR;
using WorkServices.API.Hubs;
using WorkServices.Application.Interfaces;

namespace WorkServices.API.Notifications;

public sealed class SignalRNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotifier(
        IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public Task NotifyArtisanAsync(
        Guid artisanId,
        object payload)
    {
        return _hub.Clients
            .Group($"artisan-{artisanId}")
            .SendAsync("JobAssigned", payload);
    }

    public Task NotifyCustomerAsync(
        Guid customerId,
        object payload)
    {
        return _hub.Clients
            .Group($"customer-{customerId}")
            .SendAsync("Notification", payload);
    }

    public Task NotifyAdminAsync(
        object payload)
    {
        return _hub.Clients
            .Group("admins")
            .SendAsync("Notification", payload);
    }
}