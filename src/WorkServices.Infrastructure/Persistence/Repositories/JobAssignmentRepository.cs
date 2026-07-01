using Microsoft.EntityFrameworkCore;
using WorkServices.Application.Interfaces.Repositories;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Repositories;

public sealed class JobAssignmentRepository
    : IJobAssignmentRepository
{
    private readonly ApplicationDbContext _db;

    public JobAssignmentRepository(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        JobAssignment assignment)
    {
        await _db.JobAssignments.AddAsync(
            assignment);
    }

    public async Task<JobAssignment?> GetByIdAsync(
        Guid id)
    {
        return await _db.JobAssignments
            .Include(x => x.ServiceRequest)
            .Include(x => x.Artisan)
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task<List<JobAssignment>>
        GetArtisanAssignmentsAsync(
            Guid artisanId)
    {
        return await _db.JobAssignments
            .Include(x => x.ServiceRequest)
            .Where(x => x.ArtisanId == artisanId)
            .ToListAsync();
    }

    public void Update(
        JobAssignment assignment)
    {
        _db.JobAssignments.Update(
            assignment);
    }
}