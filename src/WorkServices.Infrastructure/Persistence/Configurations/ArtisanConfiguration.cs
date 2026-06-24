using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Configurations;

public sealed class ArtisanConfiguration
    : IEntityTypeConfiguration<Artisan>
{
    public void Configure(
        EntityTypeBuilder<Artisan> builder)
    {
        builder.ToTable("Artisans");

        builder.HasBaseType<User>();

        builder.Property(x => x.ServiceType)
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .IsRequired();

        builder.Property(x => x.AverageRating)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalReviews)
            .IsRequired();
    }
}