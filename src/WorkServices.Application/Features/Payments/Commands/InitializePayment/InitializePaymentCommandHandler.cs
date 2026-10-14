
using MediatR;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Security;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.Payments.Commands.InitializePayment;

public sealed class InitializePaymentCommandHandler
    : IRequestHandler<InitializePaymentCommand, string>
{
    private readonly IPaymentRepository _payments;
    private readonly IServiceRequestRepository _serviceRequests;
    private readonly IQuoteRepository _quotes;
    private readonly IPaystackService _paystack;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public InitializePaymentCommandHandler(
        IPaymentRepository payments,
        IServiceRequestRepository serviceRequests,
        IQuoteRepository quotes,
        IPaystackService paystack,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _payments = payments;
        _serviceRequests = serviceRequests;
        _quotes = quotes;
        _paystack = paystack;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<string> Handle(
        InitializePaymentCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        if (!Enum.IsDefined(
                typeof(PaymentType),
                request.PaymentType))
        {
            throw new ValidationException(
                "Invalid payment type.");
        }

        var serviceRequest =
            await _serviceRequests.GetByIdAsync(
                request.ServiceRequestId)
            ?? throw new NotFoundException(
                "Service request not found.");

        if (serviceRequest.CustomerId != _currentUser.UserId)
        {
            throw new ForbiddenException(
                "You are not allowed to pay for this service request.");
        }

        var quote =
            await _quotes.GetByServiceRequestIdAsync(
                serviceRequest.Id)
            ?? throw new NotFoundException(
                "Quote not found for this service request.");

        if (!quote.Approved)
        {
            throw new ValidationException(
                "The quote must be approved before payment.");
        }

        decimal amount = request.PaymentType switch
        {
            PaymentType.Material => quote.MaterialCost,
            PaymentType.Labour => quote.LabourCost,
            _ => throw new ValidationException(
                "Unsupported payment type.")
        };

        if (amount <= 0)
        {
            throw new ValidationException(
                "There is no outstanding amount for this payment type.");
        }

    
        switch (request.PaymentType)
        {
            case PaymentType.Material:
                if (serviceRequest.Status !=
                    JobStatus.QuoteApproved)
                {
                    throw new ValidationException(
                        "Materials can only be paid for after quote approval.");
                }
                break;

            case PaymentType.Labour:
                if (serviceRequest.Status !=
                    JobStatus.Completed)
                {
                    throw new ValidationException(
                        "Labour can only be paid for after the work is completed.");
                }
                break;
        }

        var payment =
            await _payments.GetPendingByServiceRequestAndTypeAsync(
                serviceRequest.Id,
                request.PaymentType);

        if (payment is null)
        {
            payment = new Payment(
                serviceRequest.Id,
                amount,
                request.PaymentType);

            await _payments.AddAsync(payment);
        }
        else if (payment.Amount != amount)
        {
            throw new ValidationException(
                "The pending payment amount differs from the approved quote.");
        }

     
        if (string.IsNullOrWhiteSpace(payment.Reference))
        {
            payment.SetReference(
                $"ws_{Guid.NewGuid():N}");
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _paystack.InitializePaymentAsync(
            payment.Amount,
            serviceRequest.Customer.Email,
            payment.Reference);
    }
}
