using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public sealed class Admin : User
{
    private Admin()
    {
    }

    public Admin(
        string fullName,
        string email,
        string phoneNumber,
        string passwordHash)
        : base(
            fullName,
            email,
            phoneNumber,
            passwordHash,
            UserRole.Admin)
    {
    }
}