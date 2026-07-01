namespace WorkServices.Application.Interfaces;

public interface IRealtimeNotifier
{
    Task NotifyArtisanAsync(
        Guid artisanId,
        object message);

    Task NotifyCustomerAsync(
        Guid customerId,
        object message);

    Task NotifyAdminAsync(
        object message);
}