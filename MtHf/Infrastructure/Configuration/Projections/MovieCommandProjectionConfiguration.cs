using Domain.Entities.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Projections;

public class MovieCommandProjectionConfiguration : IEntityTypeConfiguration<MovieCommandProjection>
{
    public void Configure(EntityTypeBuilder<MovieCommandProjection> builder)
    {
        builder.ToTable(nameof(MovieCommandProjection));
        builder.HasKey(cf => new { cf.OrderId, cf.MovieId });
        builder.Property(cf => cf.OrderId).HasColumnType("uniqueidentifier").IsRequired();
        builder.Property(cf => cf.MovieId).HasColumnType("uniqueidentifier").IsRequired();

        builder.HasOne(cf => cf.Order).WithMany(c => c.MovieCommands).HasForeignKey(cf => cf.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(cf => cf.Movie).WithMany(f => f.MovieCommands).HasForeignKey(cf => cf.MovieId).OnDelete(DeleteBehavior.Cascade);
    }
}
