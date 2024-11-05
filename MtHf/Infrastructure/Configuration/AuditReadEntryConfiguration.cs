using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AuditReadEntryConfiguration : IEntityTypeConfiguration<AuditReadEntry>
{
    public void Configure(EntityTypeBuilder<AuditReadEntry> builder)
    {
        builder.ToTable(nameof(AuditReadEntry));
        builder.HasKey(x => x.Id);
    }
}
