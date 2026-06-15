using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public class Customer : User
{
    private Customer()
    {
    }

    public Customer(
        string fullName,
        string email,
        string phoneNumber)
        : base(
            fullName,
            email,
            phoneNumber,
            UserRole.Customer)
    {
    }
}