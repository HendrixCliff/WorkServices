namespace WorkServices.Application.Interfaces.Services;

public interface IPaystackService
{
    Task<string> InitializePaymentAsync(
        decimal amount,
        string email,
        string reference);

    Task<bool> VerifyPaymentAsync(
        string reference);
}