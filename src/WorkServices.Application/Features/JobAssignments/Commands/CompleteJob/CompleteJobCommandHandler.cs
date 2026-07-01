using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.JobAssignments.Commands.CompleteJob;

public sealed class CompleteJobCommandHandler
    : IRequestHandler<CompleteJobCommand>
{
    private readonly IServiceRequestRepository _serviceRequests;

    private readonly IQuoteRepository _quotes;

    private readonly IPaymentRepository _payments;

    private readonly IUnitOfWork _unitOfWork;

    public CompleteJobCommandHandler(
        IServiceRequestRepository serviceRequests,
        IQuoteRepository quotes,
        IPaymentRepository payments,
        IUnitOfWork unitOfWork)
    {
        _serviceRequests = serviceRequests;
        _quotes = quotes;
        _payments = payments;
        _unitOfWork = unitOfWork;
    }

   public async Task Handle(
    CompleteJobCommand request,
    CancellationToken cancellationToken)
{
    var serviceRequest =
        await _serviceRequests.GetByIdAsync(
            request.ServiceRequestId);

    if (serviceRequest is null)
        throw new Exception(
            "Service request not found");

    if (serviceRequest.Status != JobStatus.InProgress)
        throw new Exception(
            "Job is not in progress.");

    var quote =
        await _quotes.GetByServiceRequestIdAsync(
            request.ServiceRequestId);

    if (quote is null)
        throw new Exception(
            "Quote not found");

    serviceRequest.Complete(
        quote.ArtisanId);

    var labourPayment =
        new Payment(
            quote.ServiceRequestId,
            quote.LabourCost,
            PaymentType.Labour);

    await _payments.AddAsync(
        labourPayment);

    await _unitOfWork.SaveChangesAsync(
        cancellationToken);
}
}