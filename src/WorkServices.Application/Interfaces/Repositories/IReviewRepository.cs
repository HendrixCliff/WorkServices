using WorkServices.Domain.Entities;

namespace WorkServices.Application.Interfaces.Repositories;

public interface IReviewRepository
{
    Task AddAsync(Review review);

    Task<Review?> GetByIdAsync(Guid id);

    Task<List<Review>> GetArtisanReviewsAsync( Guid artisanId);
  
    Task<List<Review>> GetByArtisanIdAsync(Guid artisanId);
}