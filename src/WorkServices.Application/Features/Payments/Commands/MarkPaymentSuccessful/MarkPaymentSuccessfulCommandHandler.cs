using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.Payments.Commands.MarkPaymentSuccessful;

public sealed class MarkPaymentSuccessfulCommandHandler
    : IRequestHandler<MarkPaymentSuccessfulCommand>
{
    private readonly IPaymentRepository _payments;

    private readonly IServiceRequestRepository _serviceRequests;

    private readonly IUnitOfWork _unitOfWork;

    public MarkPaymentSuccessfulCommandHandler(
        IPaymentRepository payments,
        IServiceRequestRepository serviceRequests,
        IUnitOfWork unitOfWork)
    {
        _payments = payments;
        _serviceRequests = serviceRequests;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        MarkPaymentSuccessfulCommand request,
        CancellationToken cancellationToken)
    {
        var payment =
            await _payments.GetByIdAsync(request.PaymentId)
            ?? throw new Exception("Payment not found");

        var serviceRequest =
            await _serviceRequests.GetByIdAsync(
                payment.ServiceRequestId)
            ?? throw new Exception("Service request not found");

        switch (payment.Type)
        {
            case PaymentType.Material:

                payment.ConfirmMaterialPayment(
                    request.Reference);

                serviceRequest.MarkMaterialsPaid();

                break;

            case PaymentType.Labour:

                payment.ConfirmLabourPayment(
                    request.Reference);

                serviceRequest.MarkLabourPaid();

                break;
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}