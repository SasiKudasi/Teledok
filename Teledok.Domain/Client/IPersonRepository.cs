using Teledok.Contracts.Shared.DTOs;

namespace Teledok.Domain.Client;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<PersonDto?> GetByInnAsync(string inn, CancellationToken ct = default);
    Task<PersonDtoWithFounders?> GetByInnWithFoundersAsync(string inn, CancellationToken ct = default);

    Task<List<PersonDto>> GetAllAsync(CancellationToken ct = default);

    Task AddAsync(Person person, CancellationToken ct = default);
    
    Task AddFounderAsync(Founder founder, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);

    Task DeleteAsync(Person person, CancellationToken ct = default);
}
