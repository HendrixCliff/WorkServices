using WorkServices.Domain.Abstractions;

namespace WorkServices.Domain.Entities;

public class RefreshToken : Entity
{
    private RefreshToken()
    {
    }

    public Guid UserId { get; private set; }

    public string Token { get; private set; } = string.Empty;

    public DateTime ExpiresAt { get; private set; }

    public bool IsRevoked { get; private set; }

    public RefreshToken(
        Guid userId,
        string token,
        DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public void Revoke()
    {
        IsRevoked = true;

        MarkUpdated();
    }
}