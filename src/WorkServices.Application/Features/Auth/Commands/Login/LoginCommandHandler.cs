using MediatR;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Application.Interfaces.Services;
using WorkServices.Domain.Entities;
using WorkServices.Application.Common.Exceptions;

namespace WorkServices.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<
        LoginCommand,
        LoginResponse>
{
    private readonly IUserRepository _users;

    private readonly IJwtService _jwt;

    private readonly IPasswordHasher _hasher;

    private readonly IRefreshTokenRepository _tokens;

    public LoginCommandHandler(
        IUserRepository users,
        IJwtService jwt,
        IPasswordHasher hasher,
        IRefreshTokenRepository tokens)
    {
        _users = users;
        _jwt = jwt;
        _hasher = hasher;
        _tokens = tokens;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user =
            await _users.GetByEmailAsync(
                request.Email);
        

        if (user == null)
    throw new UnauthorizedAccessException("Invalid credentials.");

        if (!_hasher.Verify(
            request.Password,
            user.PasswordHash))
        {
            throw new NotFoundException(
                "Invalid credentials");
        }

        if (!user.EmailConfirmed)
    {
        throw new UnauthorizedAccessException(
            "Please confirm your email before logging in.");
    }

        var role = user switch
        {
            Customer => "Customer",
            Artisan => "Artisan",
            _ => "Admin"
        };

        var accessToken =
            _jwt.GenerateAccessToken(
                user.Id,
                user.Email,
                role);

        var refreshToken =
            _jwt.GenerateRefreshToken();

      await _tokens.AddAsync(
        new WorkServices.Domain.Entities.RefreshToken(
        user.Id,
        refreshToken,
        DateTime.UtcNow.AddDays(7)));
 
 
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}