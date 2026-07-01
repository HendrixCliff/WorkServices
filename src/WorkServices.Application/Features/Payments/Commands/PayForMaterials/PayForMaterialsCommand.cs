using MediatR;

namespace WorkServices.Application.Features.Payments.Commands.PayForMaterials;

public sealed record PayForMaterialsCommand(
    Guid PaymentId,
    string Reference)
    : IRequest;