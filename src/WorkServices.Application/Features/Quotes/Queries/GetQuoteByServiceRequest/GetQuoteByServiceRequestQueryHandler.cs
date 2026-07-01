using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.DTOs.Quotes;

namespace WorkServices.Application.Features.Quotes.Queries.GetQuoteByServiceRequest;

public sealed class GetQuoteByServiceRequestQueryHandler
    : IRequestHandler<
        GetQuoteByServiceRequestQuery,
        QuoteDto>
{
    private readonly IQuoteRepository _repository;

    public GetQuoteByServiceRequestQueryHandler(
        IQuoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<QuoteDto> Handle(
        GetQuoteByServiceRequestQuery request,
        CancellationToken cancellationToken)
    {
        var quote =
            await _repository.GetByServiceRequestIdAsync(
                request.ServiceRequestId);

        if (quote is null)
            throw new Exception("Quote not found.");

        return new QuoteDto
        {
            Id = quote.Id,
            ServiceRequestId = quote.ServiceRequestId,
            LabourCost = quote.LabourCost,
            MaterialCost = quote.MaterialCost,
            TotalCost = quote.TotalCost,
            Approved = quote.Approved
        };
    }
}