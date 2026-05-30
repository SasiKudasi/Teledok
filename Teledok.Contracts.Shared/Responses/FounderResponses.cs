namespace Teledok.Contracts.Shared.Responses;

public sealed record AddFounderResponse(Guid PersonId, Guid FounderId);

public sealed record UpdateFounderResponse(Guid PersonId, Guid FounderId);

public sealed record RemoveFounderResponse(Guid PersonId, Guid FounderId);
