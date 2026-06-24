using MediatR;
using WorkServices.Domain.Enums;

namespace WorkServices.Application.Features.Auth.Commands.RegisterArtisan;

public sealed record RegisterArtisanCommand(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password,
    ServiceType ServiceType)
    : IRequest<Guid>;