using System.Text.Json.Serialization;

namespace WorkServices.Infrastructure.DTOs.Paystack;

public sealed class InitializePaymentResponse
{
    public bool Status { get; set; }

    public string Message { get; set; } = string.Empty;

    public PaystackData Data { get; set; } = new();
}
