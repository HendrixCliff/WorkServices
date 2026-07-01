using MediatR;

namespace WorkServices.Application.Features.Quotes.Commands.ApproveQuote;

public sealed record ApproveQuoteCommand(
    Guid QuoteId)
    : IRequest;