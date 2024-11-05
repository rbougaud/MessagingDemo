using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(nameof(Customer));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.Mail).IsRequired();

        builder.HasOne(c => c.CreditCard).WithOne(cc => cc.Customer).HasForeignKey<CreditCard>(cc => cc.CustomerId).IsRequired(false);
        builder.HasOne(c => c.LoyaltyAccount) .WithOne(la => la.Customer).HasForeignKey<LoyaltyAccount>(la => la.CustomerId).IsRequired(false);
        builder.HasMany(c => c.Orders).WithOne(o => o.Customer).HasForeignKey(o => o.CustomerId);
    }
}
