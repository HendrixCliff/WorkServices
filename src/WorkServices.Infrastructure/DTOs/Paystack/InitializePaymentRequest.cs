using System.Text.Json.Serialization;

namespace WorkServices.Infrastructure.DTOs.Paystack;

public sealed class InitializePaymentRequest
{
    public string Email { get; set; } = string.Empty;

    // Paystack expects the amount in kobo
    public int Amount { get; set; }

    public string Reference { get; set; } = string.Empty;

    public string? Callback_Url { get; set; }
}