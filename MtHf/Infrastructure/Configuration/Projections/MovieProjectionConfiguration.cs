using Domain.Entities.Projections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Projections;

public class MovieProjectionConfiguration : IEntityTypeConfiguration<MovieProjection>
{
    public void Configure(EntityTypeBuilder<MovieProjection> builder)
    {
        builder.ToTable(nameof(MovieProjection));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Author).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.ReleaseDate).IsRequired();
        builder.Property(x => x.PurchasePrice).IsRequired();
    }
}
