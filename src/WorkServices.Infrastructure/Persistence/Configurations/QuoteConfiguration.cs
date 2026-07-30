using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkServices.Domain.Entities;


namespace WorkServices.Infrastructure.Persistence.Configurations;

public sealed class QuoteConfiguration
    : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MaterialCost)
            .HasColumnType("numeric(18,2)");

        builder.Property(x => x.LabourCost)
            .HasColumnType("numeric(18,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasOne<ServiceRequest>()
            .WithMany()
            .HasForeignKey(x => x.ServiceRequestId);

        builder.HasOne<Artisan>()
            .WithMany()
            .HasForeignKey(x => x.ArtisanId);
    }
}