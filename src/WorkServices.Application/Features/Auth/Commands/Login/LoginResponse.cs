namespace WorkServices.Application.Features.Auth.Commands.Login;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}