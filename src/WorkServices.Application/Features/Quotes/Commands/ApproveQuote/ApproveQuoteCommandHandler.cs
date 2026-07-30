using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Security;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Domain.Enums;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.Quotes.Commands.ApproveQuote;

public sealed class ApproveQuoteCommandHandler
    : IRequestHandler<ApproveQuoteCommand>
{
    private readonly IQuoteRepository _quotes;

    private readonly ICurrentUser _currentUser;

    private readonly IPaymentRepository _payments;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IServiceRequestRepository _serviceRequests;

    public ApproveQuoteCommandHandler(
        IQuoteRepository quotes,
        IPaymentRepository payments,
        IUnitOfWork unitOfWork,
        IServiceRequestRepository serviceRequests,
        ICurrentUser currentUser)
    {
        _quotes = quotes;
        _payments = payments;
        _unitOfWork = unitOfWork;
        _serviceRequests = serviceRequests;
        _currentUser = currentUser;
    }

  public async Task Handle(
    ApproveQuoteCommand request,
    CancellationToken cancellationToken)
{
    var quote =
        await _quotes.GetByIdAsync(request.QuoteId)
        ?? throw new NotFoundException("Quote not found");

    var serviceRequest =
        await _serviceRequests.GetByIdAsync(
            quote.ServiceRequestId)
        ?? throw new NotFoundException(
            "Service request not found");

   
    if (!_currentUser.IsAuthenticated)
    {
        throw new UnauthorizedAccessException(
            "User is not authenticated.");
    }

    if (serviceRequest.CustomerId != _currentUser.UserId)
    {
        throw new ForbiddenException(
            "You are not allowed to approve this quote.");
    }

   
    if (quote.Approved)
    {
        throw new ValidationException(
            "Quote has already been approved.");
    }

    quote.Approve();

    serviceRequest.ApproveQuote();

    if (quote.MaterialCost > 0)
    {
        var materialPayment =
            new Payment(
                quote.ServiceRequestId,
                quote.MaterialCost,
                PaymentType.Material);

        await _payments.AddAsync(materialPayment);
    }

    if (quote.LabourCost > 0)
    {
        var labourPayment =
            new Payment(
                quote.ServiceRequestId,
                quote.LabourCost,
                PaymentType.Labour);

        await _payments.AddAsync(labourPayment);
    }

    await _unitOfWork.SaveChangesAsync(cancellationToken);
}
}