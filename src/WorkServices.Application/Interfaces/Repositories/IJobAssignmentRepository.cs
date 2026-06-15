namespace WorkServices.Application.Interfaces.Repositories;

public interface IJobAssignmentRepository
{
    Task AddAsync(JobAssignment assignment);
}