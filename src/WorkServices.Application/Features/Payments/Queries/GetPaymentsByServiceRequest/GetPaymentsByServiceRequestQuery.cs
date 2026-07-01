using MediatR;
using WorkServices.Application.DTOs.Payments;
using WorkServices.Application.Common.Pagination;

namespace WorkServices.Application.Features.Payments.Queries.GetPaymentsByServiceRequest;

public sealed record GetPaymentsByServiceRequestQuery(
    Guid ServiceRequestId,
    int Page = 1,
    int PageSize = 20)
    : IRequest<PagedResult<PaymentDto>>;