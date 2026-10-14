using MediatR;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.Payments.Commands.InitializePayment;

public sealed record InitializePaymentCommand(
    Guid ServiceRequestId,
    PaymentType PaymentType)
    : IRequest<string>;
