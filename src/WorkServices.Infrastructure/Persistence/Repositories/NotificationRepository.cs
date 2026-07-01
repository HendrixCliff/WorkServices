using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;
using WorkServices.Infrastructure.Persistence;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public class NotificationRepository
    : INotificationRepository
{
    private readonly ApplicationDbContext _db;

    public NotificationRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        Notification notification)
    {
        await _db.Notifications.AddAsync(
            notification);
    }

    public async Task<List<Notification>>
        GetUserNotificationsAsync(Guid userId)
    {
        return await _db.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }


    public async Task<List<Notification>> GetByUserIdAsync(
        Guid userId)
    {
        return await _db.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}