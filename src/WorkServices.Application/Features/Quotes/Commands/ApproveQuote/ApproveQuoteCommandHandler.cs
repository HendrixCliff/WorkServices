using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;
using WorkServices.Domain.Enums;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.Quotes.Commands.ApproveQuote;

public sealed class ApproveQuoteCommandHandler
    : IRequestHandler<ApproveQuoteCommand>
{
    private readonly IQuoteRepository _quotes;

    private readonly IPaymentRepository _payments;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IServiceRequestRepository _serviceRequests;

    public ApproveQuoteCommandHandler(
        IQuoteRepository quotes,
        IPaymentRepository payments,
        IUnitOfWork unitOfWork,
        IServiceRequestRepository serviceRequests)
    {
        _quotes = quotes;
        _payments = payments;
        _unitOfWork = unitOfWork;
        _serviceRequests = serviceRequests;
    }

    public async Task Handle(
    ApproveQuoteCommand request,
    CancellationToken cancellationToken)
{
    var quote =
        await _quotes.GetByIdAsync(request.QuoteId)
        ?? throw new NotFoundException("Quote not found");

    quote.Approve();

   var requestEntity =
    await _serviceRequests.GetByIdAsync(
        quote.ServiceRequestId)
    ?? throw new NotFoundException("Service request not found");

    requestEntity!.ApproveQuote();

    var payment =
        new Payment(
            quote.ServiceRequestId,
            quote.MaterialCost,
            PaymentType.Material);

    await _payments.AddAsync(payment);

    await _unitOfWork.SaveChangesAsync(
        cancellationToken);
}
}