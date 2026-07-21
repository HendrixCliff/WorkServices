using Microsoft.Extensions.Configuration;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Services;

public sealed class EmailConfirmationService
    : IEmailConfirmationService
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
        var appUrl =
            _configuration["APP_URL"]
            ?? throw new InvalidOperationException(
                "APP_URL is missing.");

        var confirmationLink =
        $"{appUrl}/api/auth/confirm-email" +
        $"?userId={user.Id}" +
        $"&token={Uri.EscapeDataString(plainToken)}";

        var body =
            EmailTemplates.ConfirmAccount(
                user.FullName,
                confirmationLink);

        await _emailService.SendAsync(
            user.Email,
            "Confirm your Work Services account",
            body,
            true);
    }
}