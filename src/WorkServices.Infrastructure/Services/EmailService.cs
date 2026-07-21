using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using WorkServices.Application.Interfaces;
using WorkServices.Infrastructure.Persistence.Configurations;

namespace WorkServices.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public EmailService(IOptions<SmtpSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        bool isHtml = true)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("Work Services", _settings.From));
        message.To.Add(MailboxAddress.Parse(to));

        message.Subject = subject;

        message.Body = isHtml
            ? new BodyBuilder { HtmlBody = body }.ToMessageBody()
            : new TextPart("plain") { Text = body };

        using var client = new MailKit.Net.Smtp.SmtpClient();

        await client.ConnectAsync(
            _settings.Host,
            _settings.Port,
            SecureSocketOptions.SslOnConnect);

        await client.AuthenticateAsync(
            _settings.Username,
            _settings.Password);

        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }
}