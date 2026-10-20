using System.Security.Cryptography;
using MediatR;
using WorkServices.Application.Common.Security;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;

namespace WorkServices.Application.Features.Auth.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler
    : IRequestHandler<ForgotPasswordCommand, Unit>
{
    private readonly IUserRepository _users;
    private readonly IPasswordResetEmailService _passwordResetEmailService;

    public ForgotPasswordCommandHandler(
        IUserRepository users,
        IPasswordResetEmailService passwordResetEmailService)
    {
        _users = users;
        _passwordResetEmailService = passwordResetEmailService;
    }

    public async Task<Unit> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Unit.Value;
        }

        var email = request.Email.Trim();
        var user = await _users.GetByEmailAsync(email);

        // Do not reveal whether an account exists.
        if (user is null)
        {
            return Unit.Value;
        }

        // A URL-safe, cryptographically random, 256-bit token.
        var plainToken = Convert.ToHexString(
            RandomNumberGenerator.GetBytes(32));

        var tokenHash = TokenHasher.Hash(plainToken);

        user.SetPasswordResetToken(
            tokenHash,
            DateTime.UtcNow.AddMinutes(30));

        await _users.UpdateAsync(user);

        await _passwordResetEmailService.SendPasswordResetEmailAsync(
            user,
            plainToken);

        return Unit.Value;
    }
}