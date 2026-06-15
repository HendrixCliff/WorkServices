namespace WorkServices.Application.Interfaces;

public interface IRealtimeNotifier
{
    Task NotifyArtisanAsync(
        Guid artisanId,
        object payload);

    Task NotifyAdminAsync(
        string message);

    Task NotifyCustomerAsync(
        Guid customerId,
        object payload);
}