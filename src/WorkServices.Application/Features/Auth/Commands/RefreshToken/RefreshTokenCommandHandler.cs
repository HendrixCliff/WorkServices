using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;

namespace WorkServices.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    : IRequestHandler<
        RefreshTokenCommand,
        string>
{
    private readonly IRefreshTokenRepository _tokens;

    private readonly IUserRepository _users;

    private readonly IJwtService _jwt;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository tokens,
        IUserRepository users,
        IJwtService jwt)
    {
        _tokens = tokens;
        _users = users;
        _jwt = jwt;
    }

    public async Task<string> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var refreshToken =
            await _tokens.GetByTokenAsync(
                request.RefreshToken);

        if (refreshToken is null)
            throw new Exception(
                "Invalid refresh token");

        if (refreshToken.IsRevoked)
            throw new Exception(
                "Refresh token revoked");

        if (refreshToken.ExpiresAt <
            DateTime.UtcNow)
        {
            throw new Exception(
                "Refresh token expired");
        }

        var user =
            await _users.GetByIdAsync(
                refreshToken.UserId);

        if (user is null)
            throw new Exception(
                "User not found");

        var role = user switch
        {
            Customer => "Customer",
            Artisan => "Artisan",
            _ => "Admin"
        };

        return _jwt.GenerateAccessToken(
            user.Id,
            user.Email,
            role);
    }
}