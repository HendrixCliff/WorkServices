using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Repositories;

public interface IQuoteRepository
{
    Task AddAsync(Quote quote);

    Task<Quote?> GetByIdAsync(Guid id);

    void Update(Quote quote);

    Task<Quote?> GetByServiceRequestIdAsync( Guid serviceRequestId);
}