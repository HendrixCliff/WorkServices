using MediatR;

namespace WorkServices.Application.Features.Auth.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    Guid UserId,
    string Token,
    string NewPassword) : IRequest<Unit>;