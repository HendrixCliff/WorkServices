using System.Text.Json.Serialization;

namespace WorkServices.Infrastructure.DTOs.Paystack;

public sealed class VerifyPaymentResponse
{
    public bool Status { get; set; }

    public string Message { get; set; } = string.Empty;

    public VerifyPaymentData Data { get; set; } = new();
}