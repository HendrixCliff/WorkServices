using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WorkServices.API.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId =
            Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var role =
            Context.User?.FindFirst(ClaimTypes.Role)?.Value;

        if (!string.IsNullOrWhiteSpace(userId))
        {
            switch (role)
            {
                case "Artisan":
                    await Groups.AddToGroupAsync(
                        Context.ConnectionId,
                        $"artisan-{userId}");
                    break;

                case "Customer":
                    await Groups.AddToGroupAsync(
                        Context.ConnectionId,
                        $"customer-{userId}");
                    break;

                case "Admin":
                    await Groups.AddToGroupAsync(
                        Context.ConnectionId,
                        "admins");
                    break;
            }
        }

        await base.OnConnectedAsync();
    }
}