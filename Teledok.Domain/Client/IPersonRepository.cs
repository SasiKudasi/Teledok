using Teledok.Contracts.Shared.DTOs;

namespace Teledok.Domain.Client;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<PersonDto?> GetByInnAsync(string inn, CancellationToken ct = default);
    Task<PersonDtoWithFounders?> GetByInnWithFoundersAsync(string inn, CancellationToken ct = default);

    Task<List<PersonDto>> GetAllAsync(CancellationToken ct = default);

    Task AddAsync(Person person, CancellationToken ct = default);

    Task UpdateAsync(Person person, CancellationToken ct = default);

    Task DeleteAsync(Person person, CancellationToken ct = default);
}
