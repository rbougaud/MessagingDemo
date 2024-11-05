using Domain.Entities.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Projections;

public class LoyaltyAccountProjectionConfiguration : IEntityTypeConfiguration<LoyaltyAccountProjection>
{
    public void Configure(EntityTypeBuilder<LoyaltyAccountProjection> builder)
    {
        builder.ToTable(nameof(LoyaltyAccountProjection));
        builder.HasKey(x => x.Id);
        builder.Property(cf => cf.Points).IsRequired();
        builder.HasOne(la => la.Customer).WithOne(c => c.LoyaltyAccount).HasForeignKey<LoyaltyAccountProjection>(la => la.CustomerId).IsRequired();
        // Implementation soft delete
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasIndex(x => x.IsDeleted).HasFilter("IsDeleted = 0"); // Amélioration des performances
    }
}
