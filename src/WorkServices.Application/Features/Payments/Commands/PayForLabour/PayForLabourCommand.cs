using MediatR;

namespace WorkServices.Application.Features.Payments.Commands.PayForLabour;

public sealed record PayForLabourCommand(
    Guid PaymentId,
    string Reference)
    : IRequest;