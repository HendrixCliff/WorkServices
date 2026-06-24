using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WorkServices.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId =
            Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

        var role =
            Context.User?
                .FindFirst(ClaimTypes.Role)?
                .Value;

        if (!string.IsNullOrWhiteSpace(userId))
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"user-{userId}");

            if (role == "Artisan")
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"artisan-{userId}");
            }

            if (role == "Customer")
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"customer-{userId}");
            }

            if (role == "Admin")
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    "admins");
            }
        }

        await base.OnConnectedAsync();
    }
}