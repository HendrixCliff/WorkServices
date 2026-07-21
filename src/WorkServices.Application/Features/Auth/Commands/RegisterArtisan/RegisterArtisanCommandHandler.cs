using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Common.Security;


namespace WorkServices.Application.Features.Auth.Commands.RegisterArtisan;

public class RegisterArtisanCommandHandler
    : IRequestHandler<
        RegisterArtisanCommand,
        Guid>
{
    private readonly IUserRepository _users;

    private readonly IPasswordHasher _hasher;

    
    private readonly IEmailConfirmationService _emailConfirmationService;
   
    public RegisterArtisanCommandHandler(
        IUserRepository users,
        IPasswordHasher hasher,
        IEmailConfirmationService emailConfirmationService
        )
    {
        _users = users;
        _hasher = hasher;
        _emailConfirmationService = emailConfirmationService;
    }

    public async Task<Guid> Handle(
        RegisterArtisanCommand request,
        CancellationToken cancellationToken)
    {
        var exists =
            await _users.GetByEmailAsync(
                request.Email);

        if (exists != null)
            throw new NotFoundException(
                "Email already exists");

    var artisan =
    new Artisan(
        request.FullName,
        request.Email,
        request.PhoneNumber,
        _hasher.Hash(request.Password),
        request.ServiceType);

    var plainToken =
    Guid.NewGuid().ToString("N");

    var hashedToken =
    TokenHasher.Hash(plainToken);

    artisan.SetEmailConfirmationToken(
    hashedToken,
    DateTime.UtcNow.AddHours(24));

    await _users.AddAsync(artisan);

    await _emailConfirmationService
        .SendConfirmationEmailAsync(
            artisan,
            plainToken);

        return artisan.Id;
    }
}