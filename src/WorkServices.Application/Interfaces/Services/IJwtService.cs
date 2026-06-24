namespace WorkServices.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(
        Guid userId,
        string email,
        string role);

     string GenerateRefreshToken();
}