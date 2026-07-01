using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);

    Task<Payment?> GetByIdAsync(Guid id);

    Task<List<Payment>> GetByRequestIdAsync(
        Guid requestId);

    void Update(Payment payment);
    
    Task<List<Payment>> GetByServiceRequestIdAsync(Guid serviceRequestId);
}