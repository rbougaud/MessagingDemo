using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public class LoyaltyAccountConfiguration : IEntityTypeConfiguration<LoyaltyAccount>
{
    public void Configure(EntityTypeBuilder<LoyaltyAccount> builder)
    {
        builder.ToTable(nameof(LoyaltyAccount));
        builder.HasKey(x => x.Id);
        builder.Property(cf => cf.Points).IsRequired();
        builder.HasOne(la => la.Customer).WithOne(c => c.LoyaltyAccount).HasForeignKey<LoyaltyAccount>(la => la.CustomerId).IsRequired();
        // Implementation soft delete
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasIndex(x => x.IsDeleted).HasFilter("IsDeleted = 0"); // Amélioration des performances
    }
}
