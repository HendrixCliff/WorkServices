using MediatR;

namespace WorkServices.Application.Features.Quotes.Commands.CreateQuote;

public sealed record CreateQuoteCommand(
    Guid ServiceRequestId,
    decimal MaterialCost,
    decimal LabourCost,
    string Notes)
    : IRequest<Guid>;