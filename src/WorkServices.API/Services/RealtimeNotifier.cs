using Microsoft.AspNetCore.SignalR;
using WorkServices.API.Hubs;
using WorkServices.Application.Interfaces;

namespace WorkServices.API.Services;

public sealed class RealtimeNotifier
    : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hub;

    public RealtimeNotifier(
        IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public async Task NotifyArtisanAsync(
        Guid artisanId,
        object message)
    {
        await _hub.Clients
            .Group($"artisan-{artisanId}")
            .SendAsync(
                "JobAssigned",
                message);
    }

    public async Task NotifyCustomerAsync(
        Guid customerId,
        object message)
    {
        await _hub.Clients
            .Group($"customer-{customerId}")
            .SendAsync(
                "Notification",
                message);
    }

    public async Task NotifyAdminAsync(
        object message)
    {
        await _hub.Clients
            .Group("admins")
            .SendAsync(
                "Notification",
                message);
    }
}