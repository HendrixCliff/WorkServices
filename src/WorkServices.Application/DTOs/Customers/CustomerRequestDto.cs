namespace WorkServices.Application.DTOs.Customers;

public sealed class CustomerRequestDto
{
    public Guid Id { get; set; }

    public string ServiceType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}