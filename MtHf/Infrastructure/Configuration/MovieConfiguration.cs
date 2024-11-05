using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable(nameof(Movie));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Author).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.ReleaseDate).IsRequired();
        builder.Property(x => x.PurchasePrice).IsRequired();
        //builder.HasMany(m => m.MovieCommands).WithOne(cm => cm.Movie).HasForeignKey(cm => cm.MovieId);
    }
}
