using MediatR;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Security;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.Payments.Commands.MarkPaymentSuccessful;

public sealed class MarkPaymentSuccessfulCommandHandler
    : IRequestHandler<MarkPaymentSuccessfulCommand>
{
    private const string ExpectedCurrency = "NGN";

    private readonly IPaymentRepository _payments;
    private readonly IServiceRequestRepository _serviceRequests;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IPaystackService _paystack;

    public MarkPaymentSuccessfulCommandHandler(
        IPaymentRepository payments,
        IServiceRequestRepository serviceRequests,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IPaystackService paystack)
    {
        _payments = payments;
        _serviceRequests = serviceRequests;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _paystack = paystack;
    }

    public async Task Handle(
        MarkPaymentSuccessfulCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        if (string.IsNullOrWhiteSpace(request.Reference))
        {
            throw new ValidationException(
                "Payment reference is required.");
        }

        var payment = await _payments.GetByIdAsync(request.PaymentId)
            ?? throw new NotFoundException("Payment not found.");

        var serviceRequest =
            await _serviceRequests.GetByIdAsync(
                payment.ServiceRequestId)
            ?? throw new NotFoundException(
                "Service request not found.");

        if (serviceRequest.CustomerId != _currentUser.UserId)
        {
            throw new ForbiddenException(
                "You are not allowed to verify this payment.");
        }

        if (string.IsNullOrWhiteSpace(payment.Reference) ||
            !string.Equals(
                payment.Reference,
                request.Reference,
                StringComparison.Ordinal))
        {
            throw new ValidationException(
                "The supplied reference does not match the payment.");
        }

        // Idempotency: a payment already recorded as successful
        // must not trigger duplicate domain events.
        var alreadyPaid = payment.Type switch
        {
            PaymentType.Material =>
                payment.Status == PaymentStatus.MaterialPaid,

            PaymentType.Labour =>
                payment.Status == PaymentStatus.LabourPaid,

            _ => false
        };

        if (alreadyPaid)
        {
            return;
        }

        if (payment.Status != PaymentStatus.Pending)
        {
            throw new ValidationException(
                "Only pending payments can be confirmed.");
        }

        switch (payment.Type)
        {
            case PaymentType.Material:
                if (serviceRequest.Status != JobStatus.QuoteApproved)
                {
                    throw new ValidationException(
                        "Materials can only be paid for after quote approval.");
                }
                break;

            case PaymentType.Labour:
                if (serviceRequest.Status != JobStatus.Completed)
                {
                    throw new ValidationException(
                        "Labour can only be paid for after the work is completed.");
                }
                break;

            default:
                throw new ValidationException(
                    "Unsupported payment type.");
        }

        var verified = await _paystack.VerifyPaymentAsync(
            payment.Reference);

        if (verified is null || !verified.IsSuccessful)
        {
            throw new ValidationException(
                "Paystack has not confirmed a successful payment.");
        }

        if (!string.Equals(
                verified.Reference,
                payment.Reference,
                StringComparison.Ordinal))
        {
            throw new ValidationException(
                "The verified reference does not match the payment.");
        }

        if (!string.Equals(
                verified.Currency,
                ExpectedCurrency,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ValidationException(
                "The verified payment currency is incorrect.");
        }

        var expectedAmountMinorUnits = checked(
            (long)(payment.Amount * 100m));

        if (payment.Amount * 100m != expectedAmountMinorUnits ||
            verified.AmountMinorUnits != expectedAmountMinorUnits)
        {
            throw new ValidationException(
                "The verified payment amount does not match the expected amount.");
        }

        switch (payment.Type)
        {
            case PaymentType.Material:
                payment.ConfirmMaterialPayment(verified.Reference);
                serviceRequest.MarkMaterialsPaid();
                break;

            case PaymentType.Labour:
                payment.ConfirmLabourPayment(verified.Reference);
                serviceRequest.MarkLabourPaid();
                break;

            default:
                throw new ValidationException(
                    "Unsupported payment type.");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}