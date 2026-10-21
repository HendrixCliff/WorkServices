using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WorkServices.Infrastructure.DTOs.Paystack;
using WorkServices.Infrastructure.Persistence.Configurations;
using WorkServices.Application.Interfaces.Services;

namespace WorkServices.Infrastructure.Services;

public sealed class PaystackService : IPaystackService
{
    private readonly HttpClient _httpClient;
    private readonly PaystackSettings _settings;
    private readonly ILogger<PaystackService> _logger;

    public PaystackService(
        HttpClient httpClient,
        IOptions<PaystackSettings> options,
        ILogger<PaystackService> logger)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _logger = logger;

        if (!Uri.TryCreate(
                _settings.BaseUrl,
                UriKind.Absolute,
                out var baseUri))
        {
            throw new InvalidOperationException(
                "Paystack BaseUrl is missing or invalid.");
        }

        if (string.IsNullOrWhiteSpace(_settings.SecretKey))
        {
            throw new InvalidOperationException(
                "Paystack SecretKey is not configured.");
        }

        _httpClient.BaseAddress = baseUri;

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
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Payment amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Customer email is required.",
                nameof(email));
        }

        if (string.IsNullOrWhiteSpace(reference))
        {
            throw new ArgumentException(
                "Payment reference is required.",
                nameof(reference));
        }

        var amountInKobo = checked(
            (int)decimal.Round(
                amount * 100m,
                0,
                MidpointRounding.AwayFromZero));

        var request = new InitializePaymentRequest
        {
            Email = email,
            Amount = amountInKobo,
            Reference = reference,
            Callback_Url = _settings.CallbackUrl
        };

        using var response = await _httpClient.PostAsJsonAsync(
            "transaction/initialize",
            request);

        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Paystack initialization failed. HTTP status: {StatusCode}. " +
                "Response: {ResponseBody}",
                (int)response.StatusCode,
                responseBody);

            throw new HttpRequestException(
                $"Paystack initialization failed with HTTP " +
                $"{(int)response.StatusCode}: {responseBody}",
                null,
                response.StatusCode);
        }

        InitializePaymentResponse? result;

        try
        {
            result = JsonSerializer.Deserialize<InitializePaymentResponse>(
                responseBody,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (JsonException ex)
        {
            _logger.LogError(
                ex,
                "Paystack returned an invalid initialization response.");

            throw new InvalidOperationException(
                "Paystack returned an invalid response.",
                ex);
        }

        if (result is null ||
            !result.Status ||
            string.IsNullOrWhiteSpace(result.Data?.AuthorizationUrl))
        {
            _logger.LogError(
                "Paystack initialization returned an unsuccessful response: {ResponseBody}",
                responseBody);

            throw new InvalidOperationException(
                result?.Message ?? "Paystack could not initialize the payment.");
        }

        return result.Data.AuthorizationUrl;
    }

    public async Task<PaystackVerificationResult?> VerifyPaymentAsync(
        string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return null;
        }

        using var response = await _httpClient.GetAsync(
            $"transaction/verify/{Uri.EscapeDataString(reference)}");

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            _logger.LogWarning(
                "Paystack verification failed. HTTP status: {StatusCode}. " +
                "Response: {ResponseBody}",
                (int)response.StatusCode,
                responseBody);

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