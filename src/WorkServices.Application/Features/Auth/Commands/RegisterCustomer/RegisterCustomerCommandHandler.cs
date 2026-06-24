using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;

namespace WorkServices.Application.Features.Auth.Commands.RegisterCustomer;

public class RegisterCustomerCommandHandler
    : IRequestHandler<
        RegisterCustomerCommand,
        Guid>
{
    private readonly IUserRepository _users;

    private readonly IPasswordHasher _hasher;

    public RegisterCustomerCommandHandler(
        IUserRepository users,
        IPasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    public async Task<Guid> Handle(
        RegisterCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var exists =
            await _users.GetByEmailAsync(
                request.Email);

        if (exists != null)
            throw new Exception(
                "Email already exists");

        var customer =
        new Customer(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            _hasher.Hash(request.Password));

        await _users.AddAsync(customer);

        return customer.Id;
    }
}