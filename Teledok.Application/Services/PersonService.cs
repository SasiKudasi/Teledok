using Teledok.Domain.Client;

namespace Teledok.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }


    //public async Task CreatePerson()
}
