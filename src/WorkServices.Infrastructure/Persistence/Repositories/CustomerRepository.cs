using Microsoft.EntityFrameworkCore;
using WorkServices.Domain.Entities;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public class CustomerRepository
    : ICustomerRepository
{
    private readonly ApplicationDbContext _db;

    public CustomerRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Customer?> GetByIdAsync(
        Guid id)
    {
        return await _db.Customers
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(
        Customer customer)
    {
        await _db.Customers.AddAsync(customer);
    }
}