using MediatR;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Common.Security;




namespace WorkServices.Application.Features.Auth.Commands.RegisterCustomer;

public class RegisterCustomerCommandHandler
    : IRequestHandler<RegisterCustomerCommand, Unit>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
   private readonly IEmailConfirmationService _emailConfirmationService;

    public RegisterCustomerCommandHandler(
        IUserRepository users,
        IPasswordHasher hasher,
        IEmailConfirmationService emailConfirmationService
       )
    {
        _users = users;
        _hasher = hasher;
        _emailConfirmationService = emailConfirmationService;
     
    }

    public async Task<Unit> Handle(
        RegisterCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _users.GetByEmailAsync(request.Email);

        if (exists != null)
            throw new NotFoundException("Email already exists");

        var customer = new Customer(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            _hasher.Hash(request.Password));

var plainToken =
    Guid.NewGuid().ToString("N");

    var hashedToken =
    TokenHasher.Hash(plainToken);
    
    customer.SetEmailConfirmationToken(
    hashedToken,
    DateTime.UtcNow.AddHours(24));

        await _users.AddAsync(customer);

    await _emailConfirmationService
        .SendConfirmationEmailAsync(
            customer,
            plainToken);

        return Unit.Value;
    }
}