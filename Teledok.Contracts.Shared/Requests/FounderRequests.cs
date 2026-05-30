namespace Teledok.Contracts.Shared.Requests;

public sealed record AddFounderRequest(Guid PersonId, string INN, string Name);

public sealed record UpdateFounderRequest(Guid PersonId, Guid FounderId, string Name);

public sealed record RemoveFounderRequest(Guid PersonId, Guid FounderId);
