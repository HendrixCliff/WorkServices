namespace WorkServices.Application.DTOs.Payments;
public sealed class PaymentDto
{
    public Guid PaymentId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;
}