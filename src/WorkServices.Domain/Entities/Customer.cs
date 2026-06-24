using WorkServices.Domain.Enums;
using WorkServices.Domain.Entities;

namespace WorkServices.Domain.Entities;

public class Customer : User
{
    private Customer()
    {
    }
 public ICollection<ServiceRequest> ServiceRequests = new List<ServiceRequest>();
public Customer(
    string fullName,
    string email,
    string phoneNumber,
    string passwordHash)
    : base(
        fullName,
        email,
        phoneNumber,
        passwordHash,
        UserRole.Customer)
{
}
}