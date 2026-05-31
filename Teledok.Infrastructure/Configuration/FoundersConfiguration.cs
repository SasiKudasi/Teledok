using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teledok.Domain.Client;

namespace Teledok.Infrastructure.Configuration;

internal sealed class FounderConfiguration : IEntityTypeConfiguration<Founder>
{
    public void Configure(EntityTypeBuilder<Founder> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.INN)
            .IsRequired()
            .HasMaxLength(12);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.Property(x => x.PersonId)
            .IsRequired();

        builder.HasIndex(x => x.PersonId);
    }
}
