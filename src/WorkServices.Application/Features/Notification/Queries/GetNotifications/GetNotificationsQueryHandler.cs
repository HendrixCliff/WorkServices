using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.DTOs.Notification;
using WorkServices.Application.Common.Pagination;

namespace WorkServices.Application.Features.Notification.Queries.GetNotifications;

public sealed class GetNotificationsQueryHandler
    : IRequestHandler<
        GetNotificationsQuery,
        PagedResult<NotificationDto>>
{
    private readonly INotificationRepository _repository;

    public GetNotificationsQueryHandler(
        INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<NotificationDto>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications =
            await _repository.GetByUserIdAsync(
                request.UserId);

        var result = notifications
            .Select(x => new NotificationDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Message = x.Message,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            });

        return result.ToPagedResult(
            request.Page,
            request.PageSize);
    }
}