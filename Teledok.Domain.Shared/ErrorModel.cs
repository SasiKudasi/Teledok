namespace Teledok.Domain.Shared;

public class ErrorModel(ErrorType type, string? details)
{
    public ErrorType Type { get; } = type;
    public string? Details { get; } = details;
}
