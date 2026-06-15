using MediatR;

namespace WorkServices.Application.Events;

public sealed record JobAssignedEvent(
    Guid AssignmentId,
    Guid ArtisanId,
    Guid CustomerId,
    string CustomerName,
    string CustomerPhone,
    string Address,
    string ServiceType,
    string Description
) : INotification;