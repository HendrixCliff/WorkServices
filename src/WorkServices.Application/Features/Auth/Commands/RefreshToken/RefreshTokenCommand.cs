using MediatR;

namespace WorkServices.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken)
    : IRequest<string>;