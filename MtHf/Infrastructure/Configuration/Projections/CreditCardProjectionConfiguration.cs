using Domain.Entities.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Projections;

public class CreditCardProjectionConfiguration : IEntityTypeConfiguration<CreditCardProjection>
{
    public void Configure(EntityTypeBuilder<CreditCardProjection> builder)
    {
        builder.ToTable(nameof(CreditCardProjection));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.HolderName).IsRequired();
        builder.Property(x => x.CardNumber).IsRequired();
        builder.Property(x => x.CardType).IsRequired();
        builder.Property(x => x.ExpiryDate).IsRequired();
        builder.Property(x => x.MvcCode).IsRequired();

        builder.HasOne(cc => cc.Customer).WithOne(c => c.CreditCard).HasForeignKey<CreditCardProjection>(cc => cc.CustomerId).IsRequired();
    }
}
