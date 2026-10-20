using Microsoft.Extensions.Configuration;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Services;

public sealed class EmailConfirmationService : IEmailConfirmationService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public EmailConfirmationService(
        IEmailService emailService,
        IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task SendConfirmationEmailAsync(
        User user,
        string plainToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(plainToken))
        {
            throw new ArgumentException(
                "The confirmation token is required.",
                nameof(plainToken));
        }

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

        var confirmationLink =
            $"{frontendUrl}/confirm-email" +
            $"?userId={Uri.EscapeDataString(user.Id.ToString())}" +
            $"&token={Uri.EscapeDataString(plainToken)}";

        var body = EmailTemplates.ConfirmAccount(
            user.FullName,
            confirmationLink);

        await _emailService.SendAsync(
            user.Email,
            "Confirm your Work Services account",
            body,
            true);
    }
}
