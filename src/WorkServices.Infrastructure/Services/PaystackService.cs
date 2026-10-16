using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WorkServices.Infrastructure.DTOs.Paystack;
using WorkServices.Infrastructure.Persistence.Configurations;
using WorkServices.Application.Interfaces.Services;

namespace WorkServices.Infrastructure.Services;

public sealed class PaystackService : IPaystackService
{
    private readonly HttpClient _httpClient;
    private readonly PaystackSettings _settings;

    public PaystackService(
        HttpClient httpClient,
        IOptions<PaystackSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;

        _httpClient.BaseAddress =
            new Uri(_settings.BaseUrl);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _settings.SecretKey);
    }

    public async Task<string> InitializePaymentAsync(
    decimal amount,
    string email,
    string reference)
{
    var request = new InitializePaymentRequest
    {
        Email = email,
        Amount = (int)(amount * 100),
        Reference = reference,
        Callback_Url = $"{_settings.CallbackUrl}"
    };

    var response =
        await _httpClient.PostAsJsonAsync(
            "transaction/initialize",
            request);

    response.EnsureSuccessStatusCode();

    var result =
        await response.Content.ReadFromJsonAsync<
            InitializePaymentResponse>();

    if (result is null || !result.Status)
        throw new Exception("Unable to initialize payment.");

    return result.Data.AuthorizationUrl;
}
 public async Task<PaystackVerificationResult?> VerifyPaymentAsync(
    string reference)
{
    if (string.IsNullOrWhiteSpace(reference))
    {
        return null;
    }

    var response = await _httpClient.GetAsync(
        $"transaction/verify/{Uri.EscapeDataString(reference)}");

    if (!response.IsSuccessStatusCode)
    {
        return null;
    }

    var result = await response.Content
        .ReadFromJsonAsync<VerifyPaymentResponse>();

    if (result?.Status != true || result.Data is null)
    {
        return null;
    }

    var transaction = result.Data;

    return new PaystackVerificationResult(
        IsSuccessful: string.Equals(
            transaction.Status,
            "success",
            StringComparison.OrdinalIgnoreCase),
        Reference: transaction.Reference,
        AmountMinorUnits: transaction.Amount,
        Currency: transaction.Currency);
}
}