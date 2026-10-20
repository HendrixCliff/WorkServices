using Microsoft.Extensions.Configuration;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Services;

public sealed class PasswordResetEmailService : IPasswordResetEmailService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public PasswordResetEmailService(
        IEmailService emailService,
        IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(
        User user,
        string plainToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(plainToken);

        var appUrl = _configuration["APP_URL"];

        if (string.IsNullOrWhiteSpace(appUrl) ||
            !Uri.TryCreate(appUrl, UriKind.Absolute, out var appUri) ||
            (appUri.Scheme != Uri.UriSchemeHttps &&
             appUri.Scheme != Uri.UriSchemeHttp))
        {
            throw new InvalidOperationException(
                "APP_URL must be configured as an absolute frontend URL.");
        }

        var frontendUrl = appUrl.TrimEnd('/');

        var resetLink =
            $"{frontendUrl}/reset-password" +
            $"?userId={Uri.EscapeDataString(user.Id.ToString())}" +
            $"&token={Uri.EscapeDataString(plainToken)}";

        var body = PasswordResetEmailTemplate.Create(
            user.FullName,
            resetLink);

        await _emailService.SendAsync(
            user.Email,
            "Reset your Work Services password",
            body,
            isHtml: true);
    }
}