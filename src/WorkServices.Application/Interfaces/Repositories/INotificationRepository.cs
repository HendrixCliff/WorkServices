using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);

    Task<List<Notification>> GetUserNotificationsAsync(Guid userId);
}