using Microsoft.EntityFrameworkCore;
using Teledok.Domain.Client;
using Teledok.Infrastructure.ClientContext;

namespace Teledok.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{

    private readonly PersonContext _context;
    public PersonRepository(PersonContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Person person, CancellationToken ct = default)
    {
        _context.Add(person);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Person person, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Person>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Person?> GetByIdAsync(Guid id, bool includeFounders, CancellationToken ct = default)
    {
        IQueryable<Person> q =  _context.Set<Person>();
        if (includeFounders)
        {
            q = q.Include(x => x.Founders);
        }
        return await q.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Person?> GetByInnAsync(string inn, bool includeFounders, CancellationToken ct = default)
    {
        IQueryable<Person> q = _context.Set<Person>();
        if (includeFounders)
        {
            q = q.Include(x => x.Founders);
        }
        return await q.FirstOrDefaultAsync(x => x.INN == inn);
    }

    public Task UpdateAsync(Person person, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
