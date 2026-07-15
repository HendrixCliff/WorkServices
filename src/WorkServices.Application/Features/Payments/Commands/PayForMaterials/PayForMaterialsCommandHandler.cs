using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.Payments.Commands.PayForMaterials;

public sealed class PayForMaterialsCommandHandler
    : IRequestHandler<PayForMaterialsCommand>
{
    private readonly IPaymentRepository _payments;
    private readonly IUnitOfWork _unitOfWork;

    public PayForMaterialsCommandHandler(
        IPaymentRepository payments,
        IUnitOfWork unitOfWork)
    {
        _payments = payments;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        PayForMaterialsCommand request,
        CancellationToken cancellationToken)
    {
        var payment =
            await _payments.GetByIdAsync(request.PaymentId)
            ?? throw new NotFoundException("Payment not found.");

        payment.ConfirmMaterialPayment(request.Reference);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}