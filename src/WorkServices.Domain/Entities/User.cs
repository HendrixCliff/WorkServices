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
}