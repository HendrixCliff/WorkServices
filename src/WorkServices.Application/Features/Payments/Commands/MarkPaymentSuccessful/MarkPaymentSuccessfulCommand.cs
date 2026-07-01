using MediatR;

namespace WorkServices.Application.Features.Payments.Commands.MarkPaymentSuccessful;

public sealed record MarkPaymentSuccessfulCommand(
    Guid PaymentId,
    string Reference)
    : IRequest;
    