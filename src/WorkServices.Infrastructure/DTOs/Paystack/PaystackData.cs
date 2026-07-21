using System.Text.Json.Serialization;

namespace WorkServices.Infrastructure.DTOs.Paystack;

public sealed class PaystackData
{
    [JsonPropertyName("authorization_url")]
    public string AuthorizationUrl { get; set; } = string.Empty;

    [JsonPropertyName("access_code")]
    public string AccessCode { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;
}