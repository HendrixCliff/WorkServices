using WorkServices.Domain.Entities;


namespace WorkServices.Application.Interfaces.Repositories;

public interface IJobAssignmentRepository
{
    Task AddAsync(JobAssignment assignment);

    Task<JobAssignment?> GetByIdAsync(Guid id);

    void Update(JobAssignment assignment);

    Task<List<JobAssignment>>GetArtisanAssignmentsAsync( Guid artisanId);
}