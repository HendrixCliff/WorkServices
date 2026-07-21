using MediatR;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Common.Security;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Application.Features.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler
    : IRequestHandler<ConfirmEmailCommand>
{
    private readonly IUserRepository _users;

    public ConfirmEmailCommandHandler(
        IUserRepository users)
    {
        _users = users;
    }

    public async Task Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(request.UserId);

        if (user is null)
            throw new NotFoundException("User not found.");
        if (user.EmailConfirmed)
            throw new NotFoundException("Email already confirmed.");

        if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
            throw new NotFoundException("Confirmation token expired.");

        Console.WriteLine($"Request Token: {request.Token}");

        var hash = TokenHasher.Hash(request.Token);

        Console.WriteLine($"Computed Hash : {hash}");
        Console.WriteLine($"Database Hash : {user.EmailConfirmationTokenHash}");

        if (hash != user.EmailConfirmationTokenHash)
            throw new NotFoundException("Invalid confirmation token.");

        user.ConfirmEmail();

        await _users.UpdateAsync(user);
    }
}