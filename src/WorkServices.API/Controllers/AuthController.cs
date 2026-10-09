using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WorkServices.Application.Features.Auth.Commands.ConfirmEmail;
using WorkServices.Application.Features.Auth.Commands.Login;
using WorkServices.Application.Features.Auth.Commands.RefreshToken;
using WorkServices.Application.Features.Auth.Commands.RegisterArtisan;
using WorkServices.Application.Features.Auth.Commands.RegisterCustomer;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/auth")]
[EnableRateLimiting("ApiPolicy")]
public sealed class AuthController : ControllerBase
{
    private const string AccessTokenCookie = "workservices.access";
    private const string RefreshTokenCookie = "workservices.refresh";

    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register/customer")]
    public async Task<IActionResult> RegisterCustomer(
        RegisterCustomerCommand command)
    {
        await _mediator.Send(command);

        return Ok(new
        {
            message =
                "Registration successful. Please check your email to confirm your account."
        });
    }

    [HttpPost("register/artisan")]
    public async Task<IActionResult> RegisterArtisan(
        RegisterArtisanCommand command)
    {
        await _mediator.Send(command);

        return Ok(new
        {
            message =
                "Registration successful. Please check your email to confirm your account."
        });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] Guid userId,
        [FromQuery] string token)
    {
        await _mediator.Send(
            new ConfirmEmailCommand(userId, token));

        return Ok(new
        {
            message = "Email confirmed successfully."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var response = await _mediator.Send(command);

        SetAccessTokenCookie(response.AccessToken);
        SetRefreshTokenCookie(response.RefreshToken);

        return Ok(new
        {
            message = "Login successful."
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userIdValue =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var email =
            User.FindFirstValue(ClaimTypes.Email)
            ?? User.FindFirstValue("email")
            ?? string.Empty;

        var fullName =
            User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("name")
            ?? email;

        var role =
            User.FindFirstValue(ClaimTypes.Role)
            ?? User.FindFirstValue("role")
            ?? string.Empty;

        if (string.IsNullOrWhiteSpace(role))
        {
            return Unauthorized();
        }

        return Ok(new
        {
            id = userId,
            fullName,
            email,
            role
        });
    }

  [HttpPost("refresh")]
public async Task<IActionResult> Refresh()
{
    var refreshToken = Request.Cookies[RefreshTokenCookie];

    if (string.IsNullOrWhiteSpace(refreshToken))
    {
        return Unauthorized(new
        {
            message = "Refresh token is missing."
        });
    }

    var command = new RefreshTokenCommand(refreshToken);
    var newAccessToken = await _mediator.Send(command);

    SetAccessTokenCookie(newAccessToken);

    return NoContent();
}

[HttpPost("logout")]
public IActionResult Logout()
{
    Response.Cookies.Delete(
        AccessTokenCookie,
        new CookieOptions { Path = "/" });

    Response.Cookies.Delete(
        RefreshTokenCookie,
        new CookieOptions { Path = "/" });

    return NoContent();
}

private void SetAccessTokenCookie(string accessToken)
{
    Response.Cookies.Append(
        AccessTokenCookie,
        accessToken,
        new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Path = "/",
            MaxAge = TimeSpan.FromMinutes(15)
        });
}

private void SetRefreshTokenCookie(string refreshToken)
{
    Response.Cookies.Append(
        RefreshTokenCookie,
        refreshToken,
        new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            IsEssential = true,
            Path = "/",
            MaxAge = TimeSpan.FromDays(30)
        });
}
}