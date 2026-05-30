using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teledok.Application.Services;
using Teledok.Application.Shared;
using Teledok.Contracts.Shared.DTOs;
using Teledok.Contracts.Shared.Requests;
using Teledok.Contracts.Shared.Responses;
using Teledok.Domain.Shared;

namespace Teledok.Api.Controllers.Persons
{
    /// <summary>
    /// API для управления клиентами, физическими лицами и юридическими лицами с учредителями.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Получить список всех клиентов.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<PersonDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _personService.GetAllAsync(cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Получить клиента по идентификатору.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PersonDtoWithFounders), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDtoWithFounders>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _personService.GetByIdAsync(id, cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Получить клиента по ИНН.
        /// </summary>
        [HttpGet("inn/{inn}")]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDto>> GetByInn(string inn, CancellationToken cancellationToken)
        {
            var result = await _personService.GetPersonByInnAsync(inn, cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Получить юридическое лицо по ИНН вместе с учредителями.
        /// </summary>
        [HttpGet("legal-entity/inn/{inn}")]
        [ProducesResponseType(typeof(PersonDtoWithFounders), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDtoWithFounders>> GetLegalEntityByInnWithFounders(string inn, CancellationToken cancellationToken)
        {
            var result = await _personService.GetLegalEntityByInnWithFoundersAsync(inn, cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Создать индивидуального предпринимателя.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CreatePersonResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreatePersonResponse>> CreatePerson([FromBody] CreatePersonRequest request, CancellationToken cancellationToken)
        {
            var result = await _personService.CreatePersonAsync(request, cancellationToken);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
            }

            return HandleResult(result);
        }

        /// <summary>
        /// Создать юридическое лицо с учредителями.
        /// </summary>
        [HttpPost("legal-entity")]
        [ProducesResponseType(typeof(CreatePersonWithFounderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreatePersonWithFounderResponse>> CreateLegalEntity([FromBody] CreateLegalEntityRequest request, CancellationToken cancellationToken)
        {
            var result = await _personService.CreateLegalEntityAsync(request, cancellationToken);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
            }

            return HandleResult(result);
        }

        /// <summary>
        /// Обновить имя клиента.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(UpdatePersonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdatePersonResponse>> UpdatePerson([FromBody] UpdatePersonRequest request, CancellationToken cancellationToken)
        {
            var result = await _personService.UpdatePersonAsync(request, cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Удалить клиента по идентификатору.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(DeletePersonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeletePersonResponse>> DeletePerson(Guid id, CancellationToken cancellationToken)
        {
            var result = await _personService.DeletePersonAsync(id, cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Добавить учредителя юридическому лицу.
        /// </summary>
        [HttpPost("founders")]
        [ProducesResponseType(typeof(AddFounderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddFounderResponse>> AddFounder([FromBody] AddFounderRequest request, CancellationToken cancellationToken)
        {
            var result = await _personService.AddFounderAsync(request, cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Обновить данные учредителя.
        /// </summary>
        [HttpPut("founders")]
        [ProducesResponseType(typeof(UpdateFounderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateFounderResponse>> UpdateFounder([FromBody] UpdateFounderRequest request, CancellationToken cancellationToken)
        {
            var result = await _personService.UpdateFounderAsync(request, cancellationToken);
            return HandleResult(result);
        }

        /// <summary>
        /// Удалить учредителя из юридического лица.
        /// </summary>
        [HttpDelete("founders/{personId:guid}/{founderId:guid}")]
        [ProducesResponseType(typeof(RemoveFounderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RemoveFounderResponse>> RemoveFounder(Guid personId, Guid founderId, CancellationToken cancellationToken)
        {
            var request = new RemoveFounderRequest(personId, founderId);
            var result = await _personService.RemoveFounderAsync(request, cancellationToken);
            return HandleResult(result);
        }

        private ActionResult HandleResult<T>(ApplicationResult<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.Error.Type switch
            {
                ErrorType.NotFound => NotFound(new { error = result.Error.Details }),
                _ => BadRequest(new { error = result.Error.Details })
            };
        }
    }
}
