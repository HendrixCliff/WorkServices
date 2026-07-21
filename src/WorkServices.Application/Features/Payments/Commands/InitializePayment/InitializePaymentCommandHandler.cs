using MediatR;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;

namespace WorkServices.Application.Features.Payments.Commands.InitializePayment;

public sealed class InitializePaymentCommandHandler
    : IRequestHandler<InitializePaymentCommand, string>
{
    private readonly IPaymentRepository _payments;

    private readonly IServiceRequestRepository _serviceRequests;

    private readonly IPaystackService _paystack;

    private readonly IUnitOfWork _unitOfWork;

    public InitializePaymentCommandHandler(
        IPaymentRepository payments,
        IServiceRequestRepository serviceRequests,
        IPaystackService paystack,
        IUnitOfWork unitOfWork)
    {
        _payments = payments;
        _serviceRequests = serviceRequests;
        _paystack = paystack;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(
        InitializePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment =
            await _payments.GetByIdAsync(request.PaymentId)
            ?? throw new NotFoundException("Payment not found.");

        var serviceRequest =
            await _serviceRequests.GetByIdAsync(
                payment.ServiceRequestId)
            ?? throw new NotFoundException("Service request not found.");

        var customerEmail =
            serviceRequest.Customer.Email;

        var reference =
            Guid.NewGuid().ToString("N");

        payment.SetReference(reference);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return await _paystack.InitializePaymentAsync(
            payment.Amount,
            customerEmail,
            reference);
    }
}