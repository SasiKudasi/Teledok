using Microsoft.EntityFrameworkCore;
using Teledok.Domain.Client;
using Teledok.Infrastructure.Configuration;

namespace Teledok.Infrastructure.ClientContext;

public class PersonContext : DbContext
{

    public PersonContext(DbContextOptions<PersonContext> options) : base(options)
    {
    }
    public DbSet<Founder> Founders { get; init; }
    public DbSet<Person> Persons { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new FounderConfiguration());
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is null) continue;

            if (entry.Property("CreatedAt")?.CurrentValue == null)
            {
                entry.Property("CreatedAt").CurrentValue = now;
            }

            entry.Property("UpdatedAt").CurrentValue = now;

        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
