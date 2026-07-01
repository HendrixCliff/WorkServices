using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public sealed class PaymentRepository
    : IPaymentRepository
{
    private readonly ApplicationDbContext _db;

    public PaymentRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        Payment payment)
    {
        await _db.Payments.AddAsync(
            payment);
    }

    public async Task<Payment?> GetByIdAsync(
        Guid id)
    {
        return await _db.Payments
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task<List<Payment>>
        GetByRequestIdAsync(
            Guid requestId)
    {
        return await _db.Payments
            .Where(x =>
                x.ServiceRequestId ==
                requestId)
            .ToListAsync();
    }

    public void Update(
        Payment payment)
    {
        _db.Payments.Update(
            payment);
    }

    public async Task<List<Payment>> GetByServiceRequestIdAsync( Guid serviceRequestId)
    
    {
        return await _db.Payments
            .Where(x => x.ServiceRequestId == serviceRequestId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }

}