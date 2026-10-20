using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public abstract class User : Entity
{
    public string FullName { get; protected set; } = string.Empty;

    public string Email { get; protected set; } = string.Empty;

    public string PhoneNumber { get; protected set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;
   
    public string? EmailConfirmationTokenHash { get; private set; }

    public DateTime? EmailConfirmationTokenExpiry { get; private set; }

    public bool EmailConfirmed { get; private set; }

    public UserRole Role { get; protected set; }

public string? PasswordResetTokenHash { get; private set; }

public DateTime? PasswordResetTokenExpiry { get; private set; }


    public void SetEmailConfirmationToken(
    string tokenHash,
    DateTime expiry)
    {
        EmailConfirmationTokenHash = tokenHash;
        EmailConfirmationTokenExpiry = expiry;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        EmailConfirmationTokenHash = null;
        EmailConfirmationTokenExpiry = null;
    }
    protected User()
    {
    }

    protected User(
    string fullName,
    string email,
    string phoneNumber,
    string passwordHash,
    UserRole role)
{
    FullName = fullName;
    Email = email;
    PhoneNumber = phoneNumber;
    PasswordHash = passwordHash;
    Role = role;
}
 public void SetPasswordResetToken(string tokenHash, DateTime expiry)
{
    if (string.IsNullOrWhiteSpace(tokenHash))
    {
        throw new ArgumentException(
            "Token hash is required.",
            nameof(tokenHash));
    }

    if (expiry <= DateTime.UtcNow)
    {
        throw new ArgumentException(
            "Token expiry must be in the future.",
            nameof(expiry));
    }

    PasswordResetTokenHash = tokenHash;
    PasswordResetTokenExpiry = expiry;
}

public void ResetPassword(string passwordHash)
{
    if (string.IsNullOrWhiteSpace(passwordHash))
    {
        throw new ArgumentException(
            "Password hash is required.",
            nameof(passwordHash));
    }

    PasswordHash = passwordHash;
    PasswordResetTokenHash = null;
    PasswordResetTokenExpiry = null;
}
}

