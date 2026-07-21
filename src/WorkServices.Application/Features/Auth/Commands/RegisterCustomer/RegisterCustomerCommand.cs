using MediatR;

namespace WorkServices.Application.Features.Auth.Commands.RegisterCustomer;

public sealed record RegisterCustomerCommand(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password)
    : IRequest<Unit>;