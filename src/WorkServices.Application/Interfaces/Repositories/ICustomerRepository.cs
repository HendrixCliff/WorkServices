using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);

    Task AddAsync(Customer customer);
}