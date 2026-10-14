using WorkServices.Domain.Entities;
using WorkServices.Domain.Enums;
namespace WorkServices.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment);

    Task<Payment?> GetByIdAsync(Guid id);
    
Task<Payment?> GetPendingByServiceRequestAndTypeAsync(
    Guid serviceRequestId,
    PaymentType paymentType);


    Task<List<Payment>> GetByRequestIdAsync(
        Guid requestId);

    void Update(Payment payment);

    Task<Payment?> GetByReferenceAsync(string reference);
    
    Task<List<Payment>> GetByServiceRequestIdAsync(Guid serviceRequestId);
}