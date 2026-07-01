using MediatR;
using Microsoft.Extensions.Logging;
using WorkServices.Application.Interfaces;
using WorkServices.Domain.Events;

namespace WorkServices.Application.EventHandlers;

public sealed class JobAssignedDomainEventHandler
    : INotificationHandler<JobAssignedDomainEvent>
{
    private readonly ILogger<JobAssignedDomainEventHandler> _logger;
    private readonly IRealtimeNotifier _realtimeNotifier;
    private readonly IEmailService _emailService;

    public JobAssignedDomainEventHandler(
        ILogger<JobAssignedDomainEventHandler> logger,
        IRealtimeNotifier realtimeNotifier,
        IEmailService emailService)
    {
        _logger = logger;
        _realtimeNotifier = realtimeNotifier;
        _emailService = emailService;
    }

    public async Task Handle(
        JobAssignedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Job {ServiceRequestId} assigned to artisan {ArtisanId}.",
            notification.ServiceRequestId,
            notification.ArtisanId);

        await _realtimeNotifier.NotifyArtisanAsync(
            notification.ArtisanId,
            "A new job has been assigned to you.");

        await _realtimeNotifier.NotifyAdminAsync(
            $"Job {notification.ServiceRequestId} assigned.");
    }
}