using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Application.Features.Payments.Commands.PayForLabour;

public sealed class PayForLabourCommandHandler
    : IRequestHandler<PayForLabourCommand>
{
    private readonly IPaymentRepository _payments;
    private readonly IUnitOfWork _unitOfWork;

    public PayForLabourCommandHandler(
        IPaymentRepository payments,
        IUnitOfWork unitOfWork)
    {
        _payments = payments;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        PayForLabourCommand request,
        CancellationToken cancellationToken)
    {
        var payment =
            await _payments.GetByIdAsync(request.PaymentId)
            ?? throw new Exception("Payment not found.");

        payment.ConfirmLabourPayment(request.Reference);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}