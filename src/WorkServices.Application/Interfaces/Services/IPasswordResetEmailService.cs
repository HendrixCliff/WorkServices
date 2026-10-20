using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Services;

public interface IPasswordResetEmailService
{
    Task SendPasswordResetEmailAsync(
        User user,
        string plainToken);
}