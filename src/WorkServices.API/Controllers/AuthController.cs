using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.Auth.Commands.ConfirmEmail;
using WorkServices.Application.Features.Auth.Commands.Login;
using WorkServices.Application.Features.Auth.Commands.RefreshToken;
using WorkServices.Application.Features.Auth.Commands.RegisterArtisan;
using WorkServices.Application.Features.Auth.Commands.RegisterCustomer;

namespace WorkServices.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register/customer")]
    public async Task<IActionResult> RegisterCustomer(RegisterCustomerCommand command)
    {
        await _mediator.Send(command);

        return Ok(new
        {
            Message = "Registration successful. Please check your email to confirm your account."
        });
    }

    [HttpPost("register/artisan")]
    public async Task<IActionResult> RegisterArtisan(RegisterArtisanCommand command)
    {
        await _mediator.Send(command);

        return Ok(new
        {
            Message = "Registration successful. Please check your email to confirm your account."
        });
    }

   [HttpGet("confirm-email")]
public async Task<IActionResult> ConfirmEmail(
    [FromQuery] Guid userId,
    [FromQuery] string token)
{
    Console.WriteLine($"UserId: {userId}");
    Console.WriteLine($"Token : {token}");

    await _mediator.Send(new ConfirmEmailCommand(userId, token));

    return Ok(new
    {
        Message = "Email confirmed successfully."
    });
}

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<string>> Refresh(RefreshTokenCommand command)
    {
        var token = await _mediator.Send(command);

        return Ok(token);
    }
}