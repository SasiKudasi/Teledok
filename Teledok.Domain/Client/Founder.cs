using System.Diagnostics.Metrics;
using Teledok.Domain.Client.Validation;
using Teledok.Domain.Shared;

namespace Teledok.Domain.Client;

public class Founder : Entity
{
    public override Guid Id { get; init; }
    public string INN { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    internal Founder(Guid id, string inn, string fullName, DateTime created, DateTime updated)
    {
        Id = id;
        INN = inn;
        FullName = fullName;
        CreatedAt = created;
        UpdatedAt = updated;
    }
    
    internal static void ValidateFounder(Founder founder)
    {
        var validationInn = ValidationRules.CheckInn(founder.INN);
        if (validationInn != null)
        {
            founder.SetError(validationInn);
            return;
        }
        var validationName = ValidationRules.CheckName(founder.FullName);
        if (validationName != null)
        {
            founder.SetError(validationName);
            return;
        }
    }

    internal void ChangeFullName(string newFullName)
    {
        var validationName = ValidationRules.CheckName(newFullName);
        if (validationName != null)
        {
            SetError(validationName);
            return;
        }
        FullName = newFullName;
    }
}
