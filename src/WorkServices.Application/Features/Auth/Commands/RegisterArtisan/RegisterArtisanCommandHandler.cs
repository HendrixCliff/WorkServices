using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;

namespace WorkServices.Application.Features.Auth.Commands.RegisterArtisan;

public class RegisterArtisanCommandHandler
    : IRequestHandler<
        RegisterArtisanCommand,
        Guid>
{
    private readonly IUserRepository _users;

    private readonly IPasswordHasher _hasher;

    public RegisterArtisanCommandHandler(
        IUserRepository users,
        IPasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    public async Task<Guid> Handle(
        RegisterArtisanCommand request,
        CancellationToken cancellationToken)
    {
        var exists =
            await _users.GetByEmailAsync(
                request.Email);

        if (exists != null)
            throw new Exception(
                "Email already exists");

    var artisan =
    new Artisan(
        request.FullName,
        request.Email,
        request.PhoneNumber,
        _hasher.Hash(request.Password),
        request.ServiceType);
        await _users.AddAsync(artisan);

        return artisan.Id;
    }
}