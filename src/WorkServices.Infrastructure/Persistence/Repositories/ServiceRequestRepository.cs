using Microsoft.EntityFrameworkCore;
using WorkServices.Domain.Entities;
using WorkServices.Application.Interfaces.Repositories;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public class ServiceRequestRepository
    : IServiceRequestRepository
{
    private readonly ApplicationDbContext _db;

    public ServiceRequestRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceRequest?> GetByIdAsync(
        Guid id)
    {
        return await _db.ServiceRequests
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(
        ServiceRequest request)
    {
        await _db.ServiceRequests.AddAsync(request);
    }
}