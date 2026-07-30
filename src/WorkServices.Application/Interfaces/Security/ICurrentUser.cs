namespace WorkServices.Application.Interfaces.Security;

public interface ICurrentUser
{
    Guid UserId { get; }

    string Email { get; }

    string Role { get; }

    bool IsAuthenticated { get; }
}