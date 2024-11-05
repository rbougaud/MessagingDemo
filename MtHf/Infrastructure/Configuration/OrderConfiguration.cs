using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DateOrder).IsRequired();
        builder.Property(x => x.DueDate).IsRequired();
        builder.Property(x => x.DeliveryMode).IsRequired();
        //builder.HasMany(c => c.MovieCommands).WithOne(c => c.Order).HasForeignKey(o => o.OrderId);
        builder.HasOne(o => o.Customer).WithMany(c => c.Orders).HasForeignKey(o => o.CustomerId);
    }
}
