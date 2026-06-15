namespace WorkServices.Application.Interfaces.Repositories;

public interface IServiceRequestRepository
{
    Task<ServiceRequest?> GetByIdAsync(Guid id);
}