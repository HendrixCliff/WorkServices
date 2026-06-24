using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WorkServices.Application.Interfaces.Services;
 using System.Security.Cryptography;
 
namespace WorkServices.Infrastructure.Authentication;

public class JwtService : IJwtService
{
    public string GenerateAccessToken(
        Guid userId,
        string email,
        string role)
    {
        var jwtKey =
            Environment.GetEnvironmentVariable("JWT_KEY")
            ?? throw new Exception("JWT_KEY missing");

        var issuer =
            Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? throw new Exception("JWT_ISSUER missing");

        var audience =
            Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            ?? throw new Exception("JWT_AUDIENCE missing");

        var duration =
            int.Parse(
                Environment.GetEnvironmentVariable(
                    "JWT_DURATION_MINUTES")
                ?? "60");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,
                userId.ToString()),

            new(ClaimTypes.Email,
                email),

            new(ClaimTypes.Role,
                role)
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(duration),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
   

public string GenerateRefreshToken()
{
    var randomBytes = new byte[64];

    using var rng =
        RandomNumberGenerator.Create();

    rng.GetBytes(randomBytes);

    return Convert.ToBase64String(randomBytes);
}
}