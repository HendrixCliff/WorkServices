using System.Security.Cryptography;
using System.Text;
using MediatR;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Common.Security;
using WorkServices.Application.Interfaces;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Application.Features.Auth.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, Unit>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(
        IUserRepository users,
        IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new ArgumentException("A valid user ID is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Token))
        {
            throw new ArgumentException("The reset token is required.");
        }

        if (string.IsNullOrWhiteSpace(request.NewPassword) ||
            request.NewPassword.Length < 8)
        {
            throw new ArgumentException(
                "The new password must contain at least 8 characters.");
        }

        var user = await _users.GetByIdAsync(request.UserId);

        if (user is null ||
            string.IsNullOrWhiteSpace(user.PasswordResetTokenHash) ||
            user.PasswordResetTokenExpiry is null ||
            user.PasswordResetTokenExpiry.Value <= DateTime.UtcNow)
        {
            throw new InvalidOperationException(
                "The password reset link is invalid or has expired.");
        }

        var suppliedHash = TokenHasher.Hash(request.Token);
        var storedHash = user.PasswordResetTokenHash;

        var suppliedBytes = Encoding.UTF8.GetBytes(suppliedHash);
        var storedBytes = Encoding.UTF8.GetBytes(storedHash);

        var tokenMatches =
            suppliedBytes.Length == storedBytes.Length &&
            CryptographicOperations.FixedTimeEquals(
                suppliedBytes,
                storedBytes);

        if (!tokenMatches)
        {
            throw new InvalidOperationException(
                "The password reset link is invalid or has expired.");
        }

        var newPasswordHash = _passwordHasher.Hash(request.NewPassword);

        user.ResetPassword(newPasswordHash);

        await _users.UpdateAsync(user);

        return Unit.Value;
    }
}