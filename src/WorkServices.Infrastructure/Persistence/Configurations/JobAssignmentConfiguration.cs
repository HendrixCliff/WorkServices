using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Configurations;

public sealed class JobAssignmentConfiguration
    : IEntityTypeConfiguration<JobAssignment>
{
    public void Configure(
        EntityTypeBuilder<JobAssignment> builder)
    {
        builder.HasKey(x => x.Id);

       builder.HasOne(x => x.ServiceRequest)
    .WithMany(x => x.JobAssignments)
    .HasForeignKey(x => x.ServiceRequestId)
    .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(x => x.Artisan)
        .WithMany(x => x.JobAssignments)
        .HasForeignKey(x => x.ArtisanId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}