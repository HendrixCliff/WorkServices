using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WorkServices.Application.Interfaces.Security;

namespace WorkServices.Infrastructure.Authentication;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId
    {
        get
        {
            var value =
                User?.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var id)
                ? id
                : Guid.Empty;
        }
    }

    public string Email =>
        User?.FindFirstValue(ClaimTypes.Email)
        ?? string.Empty;

    public string Role =>
        User?.FindFirstValue(ClaimTypes.Role)
        ?? string.Empty;
}