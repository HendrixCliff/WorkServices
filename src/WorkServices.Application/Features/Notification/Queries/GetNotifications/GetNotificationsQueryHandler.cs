using MediatR;
using WorkServices.Application.Common.Pagination;
using WorkServices.Application.DTOs.Notification;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Common.Exceptions;

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

    public async Task<PagedResult<NotificationDto>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications =
            await _repository.GetByUserIdAsync(request.UserId);

        var ordered = notifications
            .OrderByDescending(x => x.CreatedAt);

        var total = ordered.Count();

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new NotificationDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Message = x.Message,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        return new PagedResult<NotificationDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = total
        };
    }
}