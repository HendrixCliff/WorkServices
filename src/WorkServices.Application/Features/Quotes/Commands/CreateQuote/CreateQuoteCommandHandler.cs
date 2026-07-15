using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.Quotes.Commands.CreateQuote;

public sealed class CreateQuoteCommandHandler
    : IRequestHandler<CreateQuoteCommand, Guid>
{
    private readonly IQuoteRepository _quotes;
    private readonly IServiceRequestRepository _serviceRequests;
    private readonly IUnitOfWork _unitOfWork;

    public CreateQuoteCommandHandler(
        IQuoteRepository quotes,
        IServiceRequestRepository serviceRequests,
        IUnitOfWork unitOfWork)
    {
        _quotes = quotes;
        _serviceRequests = serviceRequests;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
    CreateQuoteCommand request,
    CancellationToken cancellationToken)
{
    var serviceRequest =
        await _serviceRequests.GetByIdAsync(
            request.ServiceRequestId);

    if (serviceRequest is null)
        throw new NotFoundException(
            "Service request not found");

    var quote =
        new Quote(
            request.ServiceRequestId,
            request.ArtisanId,
            request.MaterialCost,
            request.LabourCost,
            request.Notes);

    serviceRequest.SetEstimatedCost(
    quote.TotalCost);

    serviceRequest.SubmitQuote();

    await _quotes.AddAsync(
        quote);

    await _unitOfWork.SaveChangesAsync(
        cancellationToken);

    return quote.Id;
}
}