using Domain.Entities.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Projections;

public class CustomerProjectionConfiguration : IEntityTypeConfiguration<CustomerProjection>
{
    public void Configure(EntityTypeBuilder<CustomerProjection> builder)
    {
        builder.ToTable(nameof(CustomerProjection));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.Mail).IsRequired();

        builder.HasOne(c => c.CreditCard).WithOne(cc => cc.Customer).HasForeignKey<CreditCardProjection>(cc => cc.CustomerId).IsRequired(false);
        builder.HasOne(c => c.LoyaltyAccount).WithOne(la => la.Customer).HasForeignKey<LoyaltyAccountProjection>(la => la.CustomerId).IsRequired(false);
        builder.HasMany(c => c.Orders).WithOne(o => o.Customer).HasForeignKey(o => o.CustomerId);
    }
}
