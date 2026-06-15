using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public abstract class User : Entity
{
    public string FullName { get; protected set; } = string.Empty;

    public string Email { get; protected set; } = string.Empty;

    public string PhoneNumber { get; protected set; } = string.Empty;

    public UserRole Role { get; protected set; }

    protected User()
    {
    }

    protected User(
        string fullName,
        string email,
        string phoneNumber,
        UserRole role)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
    }
}