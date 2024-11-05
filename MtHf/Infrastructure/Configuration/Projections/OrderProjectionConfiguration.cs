using Domain.Entities.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Projections;

public class OrderProjectionConfiguration : IEntityTypeConfiguration<OrderProjection>
{
    public void Configure(EntityTypeBuilder<OrderProjection> builder)
    {
        builder.ToTable(nameof(OrderProjection));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DateOrder).IsRequired();
        builder.Property(x => x.DueDate).IsRequired();
        builder.Property(x => x.DeliveryMode).IsRequired();

        builder.HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId);
    }
}
