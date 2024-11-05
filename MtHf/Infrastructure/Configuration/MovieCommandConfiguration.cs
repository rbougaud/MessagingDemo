using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public class MovieCommandConfiguration : IEntityTypeConfiguration<MovieCommand>
{
    public void Configure(EntityTypeBuilder<MovieCommand> builder)
    {
        builder.ToTable(nameof(MovieCommand));
        builder.HasKey(cf => new { cf.OrderId, cf.MovieId });
        builder.Property(cf => cf.OrderId).HasColumnType("uniqueidentifier").IsRequired();
        builder.Property(cf => cf.MovieId).HasColumnType("uniqueidentifier").IsRequired();

        builder.HasOne(cf => cf.Order).WithMany(c => c.MovieCommands).HasForeignKey(cf => cf.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(cf => cf.Movie).WithMany(f => f.MovieCommands).HasForeignKey(cf => cf.MovieId).OnDelete(DeleteBehavior.Cascade);
    }
}
