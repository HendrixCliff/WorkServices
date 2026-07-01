using MediatR;
using WorkServices.Application.Common.Pagination;
using WorkServices.Application.DTOs.Notification;

namespace WorkServices.Application.Features.Notification.Queries.GetNotifications;
public sealed record GetNotificationsQuery(
    Guid UserId,
    int Page = 1,
    int PageSize = 20)
    : IRequest<PagedResult<NotificationDto>>;