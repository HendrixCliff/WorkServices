using MediatR;
using WorkServices.Application.Events;
using WorkServices.Application.Interfaces;

namespace WorkServices.Application.EventHandlers;

public class JobAssignedEventHandler
    : INotificationHandler<JobAssignedEvent>
{
    private readonly IRealtimeNotifier _realtimeNotifier;

    private readonly IEmailService _emailService;

    public JobAssignedEventHandler(
        IRealtimeNotifier realtimeNotifier,
        IEmailService emailService)
    {
        _realtimeNotifier = realtimeNotifier;
        _emailService = emailService;
    }

    public async Task Handle(
        JobAssignedEvent notification,
        CancellationToken cancellationToken)
    {
        await _realtimeNotifier.NotifyArtisanAsync(
            notification.ArtisanId,
            notification);

        await _realtimeNotifier.NotifyCustomerAsync(
            notification.CustomerId,
            "Artisan assigned successfully.");

        await _realtimeNotifier.NotifyAdminAsync(
            "A new job has been assigned.");

        await _emailService.SendAsync(
            "artisan@email.com",
            "New Job Assignment",
            $"""
            Customer: {notification.CustomerName}

            Phone: {notification.CustomerPhone}

            Address: {notification.Address}

            Service: {notification.ServiceType}

            Description: {notification.Description}
            """);
    }
}