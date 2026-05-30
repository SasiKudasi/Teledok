namespace Teledok.Domain.Client;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(Guid id, bool includeFounders, CancellationToken ct = default);

    Task<Person?> GetByInnAsync(string inn, bool includeFounders, CancellationToken ct = default);

    Task<List<Person>> GetAllAsync(CancellationToken ct = default);

    Task AddAsync(Person person, CancellationToken ct = default);

    Task UpdateAsync(Person person, CancellationToken ct = default);

    Task DeleteAsync(Person person, CancellationToken ct = default);
}
