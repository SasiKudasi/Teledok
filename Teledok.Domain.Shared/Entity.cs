
using System.Security.Cryptography;

namespace Teledok.Domain.Shared;

public abstract class Entity : IEntity
{
    private ErrorModel? _error;
    public abstract Guid Id { get; init; }

    protected void SetError(string error)
    {
        _error = new ErrorModel(ErrorType.ValidationError, error);
    }

    public bool IsValid => _error == null;
    public ErrorModel? Error => _error;
}


