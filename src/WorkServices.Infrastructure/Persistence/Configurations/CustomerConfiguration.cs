using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkServices.Domain.Entities;

namespace WorkServices.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration :
    IEntityTypeConfiguration<Customer>
{
    public void Configure(
        EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasBaseType<User>();
    }
}