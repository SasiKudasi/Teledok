using Microsoft.EntityFrameworkCore;
using Teledok.Contracts.Shared.DTOs;
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
        await _context.SaveChangesAsync(ct);
    }

    public async Task AddFounderAsync(Founder founder, CancellationToken ct = default)
    {
        _context.Add(founder);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Person person, CancellationToken ct = default)
    {
        _context.Remove(person);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<PersonDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .Select(x =>
                 new PersonDto
                 {
                     Id = x.Id,
                     Inn = x.INN,
                     Name = x.Name,
                     Type = x.Type,
                     CreatedAt = x.CreatedAt,
                     UpdatedAt = x.UpdatedAt
                 }
            )
            .ToListAsync(ct);
    }

    public async Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context
            .Persons
            .Include(x => x.Founders)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<PersonDto?> GetByInnAsync(string inn, CancellationToken ct = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .Where(x => x.INN == inn)
            .Select(x => new PersonDto
            {
                Inn = x.INN,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Type = x.Type,
            })
        .FirstOrDefaultAsync(ct);
    }


    public async Task<PersonDtoWithFounders?> GetByInnWithFoundersAsync(string inn, CancellationToken ct = default)
    {
        return await _context.Persons
            .AsNoTracking()
            .Where(x => x.INN == inn)
            .Include(x => x.Founders)
            .Select(x => new PersonDtoWithFounders
            {
                Inn = x.INN,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Type = x.Type,
                Founders = x.Founders.Select(f => new FounderDto
                {
                    INN = f.INN,
                    FullName = f.FullName,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                }).ToList()
            })
        .FirstOrDefaultAsync(ct);
    }


    public async Task SaveChangesAsync( CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }

}
