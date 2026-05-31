using Teledok.Contracts.Shared.Enums;
using Teledok.Domain.Client.Validation;
using Teledok.Domain.Shared;

namespace Teledok.Domain.Client;

public class Person : Entity
{
    public override Guid Id { get; init; }

    public string INN { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public ClientType Type { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    private readonly List<Founder> _founders = [];
    public IReadOnlyCollection<Founder> Founders => _founders;

    private Person()
    {
        
    }
    private Person(Guid id, string inn, string name, ClientType type, DateTime created, DateTime updated)
    {
        Id = id;
        INN = inn;
        Name = name;
        Type = type;
        CreatedAt = created;
        UpdatedAt = updated;
    }


    public static Person Create(Guid? id, string inn, string name, ClientType type, DateTime created, DateTime updated)
    {
        var person = new Person(
            id ?? Guid.NewGuid(),
            inn,
            name,
            type,
            created,
            updated);

        ValidatePerson(person);

        return person;
    }

    public static Founder CreateFounder(Guid? id, Guid personId, string inn, string fullName, DateTime created, DateTime updated)
    {
        var founder = Founder.Create(
            id ?? Guid.NewGuid(),
            personId,
            inn,
            fullName,
            created,
            updated);

        Founder.ValidateFounder(founder);

        return founder;
    }

    public void AddFounder(Founder founder)
    {
        var validation = ValidationRules.CanAddFounder(Type, _founders, founder.INN);
        if (validation != null)
        {
            SetError(validation);
            return;
        }
        _founders.Add(founder);
    }

    private static void ValidatePerson(Person person)
    {
        var validationInn = ValidationRules.CheckInn(person.INN);
        if (validationInn != null)
        {
            person.SetError(validationInn);
            return;
        }
        var validationName = ValidationRules.CheckName(person.Name);
        if (validationName != null)
        {
            person.SetError(validationName);
            return;
        }
    }

    public void ChangeName(string newName)
    {
        var validationResult = ValidationRules.CheckName(newName);
        if (validationResult != null)
        {
            SetError(validationResult);
            return;
        }

        Name = newName;
    }

    public void ChangeFounderFullName(Founder founder, string fullName)
    {
        founder.ChangeFullName(fullName);
    }

    public void RemoveFounder(string founderInn)
    {
        var founder = _founders.FirstOrDefault(f => f.INN == founderInn);
        if (founder != null)
        {
            _founders.Remove(founder);
        }
    }

}
