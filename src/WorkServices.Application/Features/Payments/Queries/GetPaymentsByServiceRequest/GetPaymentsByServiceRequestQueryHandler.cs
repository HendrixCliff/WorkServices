using MediatR;
using WorkServices.Application.Common;
using WorkServices.Application.DTOs.Payments;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Common.Pagination;

namespace WorkServices.Application.Features.Payments.Queries.GetPaymentsByServiceRequest;

public sealed class GetPaymentsByServiceRequestQueryHandler
    : IRequestHandler<
        GetPaymentsByServiceRequestQuery,
        PagedResult<PaymentDto>>
{
    private readonly IPaymentRepository _repository;

    public GetPaymentsByServiceRequestQueryHandler(
        IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<PaymentDto>> Handle(
        GetPaymentsByServiceRequestQuery request,
        CancellationToken cancellationToken)
    {
        var payments =
            await _repository.GetByServiceRequestIdAsync(
                request.ServiceRequestId);

        var ordered = payments
            .OrderBy(x => x.CreatedAt);

        var total = ordered.Count();

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(payment => new PaymentDto
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Type = payment.Type.ToString(),
                Status = payment.Status.ToString(),
                Reference = payment.Reference
            })
            .ToList();

        return new PagedResult<PaymentDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = total
        };
    }
}