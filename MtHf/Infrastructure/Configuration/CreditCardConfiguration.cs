using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        builder.ToTable(nameof(CreditCard));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.HolderName).IsRequired();
        builder.Property(x => x.CardNumber).IsRequired();
        builder.Property(x => x.CardType).IsRequired();
        builder.Property(x => x.ExpiryDate).IsRequired();
        builder.Property(x => x.MvcCode).IsRequired();

        builder.HasOne(cc => cc.Customer).WithOne(c => c.CreditCard).HasForeignKey<CreditCard>(cc => cc.CustomerId).IsRequired();
    }
}
