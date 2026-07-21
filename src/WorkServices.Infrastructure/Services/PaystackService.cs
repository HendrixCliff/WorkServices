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
    public async Task<bool> VerifyPaymentAsync(
    string reference)
{
    var response =
        await _httpClient.GetAsync(
            $"transaction/verify/{reference}");

    response.EnsureSuccessStatusCode();

    var result =
        await response.Content.ReadFromJsonAsync<VerifyPaymentResponse>();

    if (result is null)
        return false;

    return result.Status &&
           result.Data.Status.Equals(
               "success",
               StringComparison.OrdinalIgnoreCase);
}
}