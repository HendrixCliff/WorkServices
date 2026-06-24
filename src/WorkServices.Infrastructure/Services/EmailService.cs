using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using WorkServices.Application.Interfaces;
using WorkServices.Infrastructure.Configurations;

namespace WorkServices.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public EmailService(
        IOptions<SmtpSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        bool isHtml = true)
    {
        using var smtp = new SmtpClient
        {
            Host = _settings.Host,
            Port = _settings.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _settings.Username,
                _settings.Password)
        };

        var mail = new MailMessage
        {
            From = new MailAddress(
                _settings.From,
                "Work Services"),

            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };

        mail.To.Add(to);

        await smtp.SendMailAsync(mail);
    }
}