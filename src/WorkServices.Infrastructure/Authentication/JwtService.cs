using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WorkServices.Application.Common.Exceptions;
using WorkServices.Application.Interfaces.Services;

namespace WorkServices.Infrastructure.Authentication;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(
        Guid userId,
        string email,
        string role)
    {
        var jwtKey =
            _configuration["JWT_KEY"]
            ?? throw new NotFoundException("JWT_KEY missing");

        var issuer =
            _configuration["JWT_ISSUER"]
            ?? throw new NotFoundException("JWT_ISSUER missing");

        var audience =
            _configuration["JWT_AUDIENCE"]
            ?? throw new NotFoundException("JWT_AUDIENCE missing");

        var duration =
            int.Parse(
                _configuration["JWT_DURATION_MINUTES"] ?? "60");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(duration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }
}