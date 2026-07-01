using MediatR;
using WorkServices.Application.DTOs.Quotes;

namespace WorkServices.Application.Features.Quotes.Queries.GetQuoteByServiceRequest;

public sealed record GetQuoteByServiceRequestQuery(
    Guid ServiceRequestId)
    : IRequest<QuoteDto>;