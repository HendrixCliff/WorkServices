using System.Text.Json.Serialization;

namespace WorkServices.Infrastructure.DTOs.Paystack;

public sealed class VerifyPaymentData
{
    public string Status { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public int Amount { get; set; }
}