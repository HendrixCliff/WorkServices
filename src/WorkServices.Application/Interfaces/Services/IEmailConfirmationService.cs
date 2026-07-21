using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Services;

public interface IEmailConfirmationService
{
    Task SendConfirmationEmailAsync(
        User user,
        string plainToken);
}