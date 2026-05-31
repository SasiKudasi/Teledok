using Teledok.Application.Shared;
using Teledok.Contracts.Shared.DTOs;
using Teledok.Contracts.Shared.Enums;
using Teledok.Contracts.Shared.Requests;
using Teledok.Contracts.Shared.Responses;
using Teledok.Domain.Client;

namespace Teledok.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }


    public async Task<ApplicationResult<CreatePersonResponse>> CreatePersonAsync(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var person = Person.Create(
            null,
            request.INN,
            request.Name,
            ClientType.IndividualEntrepreneur,
            DateTime.UtcNow,
            DateTime.UtcNow);

        if (!person.IsValid)
        {
            return ApplicationResult<CreatePersonResponse>.Fail(person.Error!.Details!);
        }

        await _personRepository.AddAsync(person);
        return ApplicationResult<CreatePersonResponse>.Success(new CreatePersonResponse(person.Id));

    }


    public async Task<ApplicationResult<CreatePersonWithFounderResponse>> CreateLegalEntityAsync(CreateLegalEntityRequest request, CancellationToken cancellationToken)
    {

        var person = Person.Create(
            null,
            request.INN,
            request.Name,
            ClientType.LegalEntity,
            DateTime.UtcNow,
            DateTime.UtcNow);

        if (!person.IsValid)
        {
            return ApplicationResult<CreatePersonWithFounderResponse>.Fail(person.Error!.Details!);
        }

        foreach (var founderReq in request.Founders)
        {
            person.AddFounder(Founder.Create(null, person.Id, founderReq.INN, founderReq.Name, DateTime.UtcNow, DateTime.UtcNow));

            if (!person.IsValid)
            {
                return ApplicationResult<CreatePersonWithFounderResponse>.Fail(person.Error!.Details!);
            }
        }

        await _personRepository.AddAsync(person, cancellationToken);

        return ApplicationResult<CreatePersonWithFounderResponse>.Success(new CreatePersonWithFounderResponse(person.Id));

    }

    public async Task<ApplicationResult<List<PersonDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var persons = await _personRepository.GetAllAsync(cancellationToken);
        return ApplicationResult<List<PersonDto>>.Success(persons);
    }

    public async Task<ApplicationResult<PersonDtoWithFounders>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(id, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<PersonDtoWithFounders>.NotFound($"Person with id: {id} not found");
        }


        var result = new PersonDtoWithFounders
        {
            Id = person.Id,
            Inn = person.INN,
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt,
            Type = person.Type,
            Founders = person.Type == ClientType.LegalEntity ?
                person.Founders
                .Select(x =>
                {
                    return new FounderDto
                    {
                        INN = x.INN,
                        FullName = x.FullName,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt
                    };
                }).ToList() : new List<FounderDto>()
        };

        return ApplicationResult<PersonDtoWithFounders>.Success(result);

    }

    public async Task<ApplicationResult<PersonDto>> GetPersonByInnAsync(string inn, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByInnAsync(inn, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<PersonDto>.NotFound($"Person with INN: {inn} not found");
        }
        return ApplicationResult<PersonDto>.Success(person);
    }

    public async Task<ApplicationResult<PersonDtoWithFounders>> GetLegalEntityByInnWithFoundersAsync(string inn, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByInnWithFoundersAsync(inn, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<PersonDtoWithFounders>.NotFound($"Person with INN: {inn} not found");
        }
        return ApplicationResult<PersonDtoWithFounders>.Success(person);
    }

    public async Task<ApplicationResult<UpdatePersonResponse>> UpdatePersonAsync(UpdatePersonRequest request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<UpdatePersonResponse>.NotFound($"Person with id: {request.Id} not found");
        }

        person.ChangeName(request.Name);

        if (!person.IsValid)
        {
            return ApplicationResult<UpdatePersonResponse>.Fail(person.Error!.Details!);
        }

        await _personRepository.SaveChangesAsync(cancellationToken);
        return ApplicationResult<UpdatePersonResponse>.Success(new UpdatePersonResponse(person.Id));
    }

    public async Task<ApplicationResult<DeletePersonResponse>> DeletePersonAsync(Guid id, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(id, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<DeletePersonResponse>.NotFound($"Person with id: {id} not found");
        }

        await _personRepository.DeleteAsync(person, cancellationToken);
        return ApplicationResult<DeletePersonResponse>.Success(new DeletePersonResponse(person.Id));
    }

    public async Task<ApplicationResult<AddFounderResponse>> AddFounderAsync(AddFounderRequest request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<AddFounderResponse>.NotFound($"Person with id: {request.PersonId} not found");
        }

        var founder = Founder.Create(null, person.Id, request.INN, request.Name, DateTime.UtcNow, DateTime.UtcNow);
        person.AddFounder(founder);

        if (!person.IsValid)
        {
            return ApplicationResult<AddFounderResponse>.Fail(person.Error!.Details!);
        }

        await _personRepository.AddFounderAsync(founder, cancellationToken);
        return ApplicationResult<AddFounderResponse>.Success(new AddFounderResponse(person.Id, founder.Id));
    }

    public async Task<ApplicationResult<UpdateFounderResponse>> UpdateFounderAsync(UpdateFounderRequest request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<UpdateFounderResponse>.NotFound($"Person with id: {request.PersonId} not found");
        }

        var founder = person.Founders.FirstOrDefault(f => f.INN == request.FounderInn);
        if (founder is null)
        {
            return ApplicationResult<UpdateFounderResponse>.NotFound($"Founder with id: {request.FounderInn} not found");
        }

        person.ChangeFounderFullName(founder, request.Name);

        if (!person.IsValid)
        {
            return ApplicationResult<UpdateFounderResponse>.Fail(person.Error!.Details!);
        }

        await _personRepository.SaveChangesAsync(cancellationToken);
        return ApplicationResult<UpdateFounderResponse>.Success(new UpdateFounderResponse(person.Id, founder.Id));
    }

    public async Task<ApplicationResult<RemoveFounderResponse>> RemoveFounderAsync(RemoveFounderRequest request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            return ApplicationResult<RemoveFounderResponse>.NotFound($"Person with id: {request.PersonId} not found");
        }

        var founder = person.Founders.FirstOrDefault(f => f.INN == request.FounderInn);
        if (founder is null)
        {
            return ApplicationResult<RemoveFounderResponse>.NotFound($"Founder with id: {request.FounderInn} not found");
        }

        person.RemoveFounder(request.FounderInn);
        await _personRepository.SaveChangesAsync(cancellationToken);
        return ApplicationResult<RemoveFounderResponse>.Success(new RemoveFounderResponse(person.Id, founder.Id));
    }

}
