using Microsoft.EntityFrameworkCore;
using WorkServices.Domain.Entities;
using WorkServices.Domain.Abstractions;

namespace WorkServices.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
  
    public DbSet<User> Users => Set<User>();

    public DbSet<Admin> Admins => Set<Admin>();
      
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Artisan> Artisans => Set<Artisan>();

    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

    public DbSet<JobAssignment> JobAssignments => Set<JobAssignment>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<Notification> Notifications => Set<Notification>();
   
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Ignore<DomainEvent>();

    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(ApplicationDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
}
}