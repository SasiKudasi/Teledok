
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teledok.Domain.Client;

namespace Teledok.Infrastructure.Configuration
{
    internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Name).IsRequired();
            
            builder.Property(x => x.INN).IsRequired().HasMaxLength(12);
            
            builder.HasIndex(x => x.INN).IsUnique();

            builder.Property(x => x.Type)
                .IsRequired();
            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired();

            builder.HasMany(x => x.Founders)
                .WithOne()
                .HasForeignKey("PersonId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata
                .FindNavigation(nameof(Person.Founders))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);


            builder.Ignore(x => x.Error);

        }
    }
}
