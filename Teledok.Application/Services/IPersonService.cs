using Teledok.Application.Shared;
using Teledok.Contracts.Shared.DTOs;
using Teledok.Contracts.Shared.Requests;
using Teledok.Contracts.Shared.Responses;

namespace Teledok.Application.Services;

public interface IPersonService
{
    Task<ApplicationResult<CreatePersonResponse>> CreatePersonAsync(CreatePersonRequest request, CancellationToken cancellationToken);
    Task<ApplicationResult<CreatePersonWithFounderResponse>> CreateLegalEntityAsync(CreateLegalEntityRequest request, CancellationToken cancellationToken);
    Task<ApplicationResult<List<PersonDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApplicationResult<GetPersonByIdRequest>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ApplicationResult<PersonDto>> GetPersonByInnAsync(string inn, CancellationToken cancellationToken);
    Task<ApplicationResult<PersonDtoWithFounders>> GetLegalEntityByInnWithFoundersAsync(string inn, CancellationToken cancellationToken);
}
