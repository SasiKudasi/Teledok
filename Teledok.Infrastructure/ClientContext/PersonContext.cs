using Microsoft.EntityFrameworkCore;
using Teledok.Domain.Client;
using Teledok.Infrastructure.Configuration;

namespace Teledok.Infrastructure.ClientContext;

public class PersonContext : DbContext
{

    public PersonContext(DbContextOptions<PersonContext> options) : base(options)
    {
    }
    public DbSet<Person> Persons { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new FounderConfiguration());
    }
}
