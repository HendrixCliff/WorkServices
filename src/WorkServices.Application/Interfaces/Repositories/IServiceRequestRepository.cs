using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Repositories;

public interface IServiceRequestRepository
{
    Task<ServiceRequest?> GetByIdAsync(Guid id);

    Task AddAsync(ServiceRequest request);
}