using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public class JobAssignmentRepository
    : IJobAssignmentRepository
{
    private readonly ApplicationDbContext _db;

    public JobAssignmentRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<JobAssignment?> GetByIdAsync(
        Guid id)
    {
        return await _db.JobAssignments
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(
        JobAssignment assignment)
    {
        await _db.JobAssignments.AddAsync(
            assignment);
    }

    public void Update(
        JobAssignment assignment)
    {
        _db.JobAssignments.Update(
            assignment);
    }
}