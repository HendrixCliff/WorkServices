namespace WorkServices.Application.Interfaces.Services;

public sealed record PaystackVerificationResult(
    bool IsSuccessful,
    string Reference,
    long AmountMinorUnits,
    string Currency);