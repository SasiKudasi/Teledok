namespace Teledok.Contracts.Shared.DTOs;

public sealed class PersonDtoWithFounders : PersonDto
{
    public IReadOnlyCollection<FounderDto> Founders { get; init; } = Array.Empty<FounderDto>();

}
