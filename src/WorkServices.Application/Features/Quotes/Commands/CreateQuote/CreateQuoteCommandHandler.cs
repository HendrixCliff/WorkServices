using MediatR;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Security;
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
    private readonly ICurrentUser _currentUser;
    private readonly IJobAssignmentRepository _assignments;

    public CreateQuoteCommandHandler(
        IQuoteRepository quotes,
        IServiceRequestRepository serviceRequests,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        IJobAssignmentRepository assignments)
    {
        _quotes = quotes;
        _serviceRequests = serviceRequests;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _assignments = assignments;
    }

    public async Task<Guid> Handle(
    CreateQuoteCommand request,
    CancellationToken cancellationToken)
{
    var serviceRequest =
        await _serviceRequests.GetByIdAsync(
            request.ServiceRequestId);

 var assignment =
    await _assignments.GetByServiceRequestIdAsync(
        request.ServiceRequestId)
    ?? throw new ValidationException(
        "No artisan has been assigned.");

     if (serviceRequest is null)
        throw new NotFoundException(
            "Service request not found");
    
        
    if (assignment.ArtisanId != _currentUser.UserId)
    {
        throw new ForbiddenException(
            "You are not assigned to this service request.");
    }
 

   var quote = 
   new Quote(
    request.ServiceRequestId,
    _currentUser.UserId,
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