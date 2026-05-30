namespace Teledok.Contracts.Shared.Requests;

public sealed record CreateLegalEntityRequest(string INN, string Name, IReadOnlyCollection<CreateFounderRequest> Founders);

public sealed record CreateFounderRequest(string INN, string Name);

