using MediatR;

namespace WorkServices.Application.Features.Payments.Commands.InitializePayment;

public sealed record InitializePaymentCommand(
    Guid PaymentId)
    : IRequest<string>;