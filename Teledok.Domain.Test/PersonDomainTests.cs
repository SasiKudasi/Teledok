using Teledok.Contracts.Shared.Enums;
using Teledok.Domain.Client;
using Xunit;

namespace Teledok.Domain.Tests;

public class PersonDomainTests
{
    [Fact]
    public void Create_IndividualEntrepreneur_With12DigitsInn_IsValid()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "123456789012", "Ivan Ivanov", ClientType.IndividualEntrepreneur, now, now);

        Assert.True(person.IsValid);
        Assert.Null(person.Error);
        Assert.Equal("123456789012", person.INN);
    }

    [Fact]
    public void Create_IndividualEntrepreneur_With10DigitsInn_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Ivan Ivanov", ClientType.IndividualEntrepreneur, now, now);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("Individual entrepreneur INN must be 12 digits", person.Error!.Details!);
    }

    [Fact]
    public void AddFounder_ToIndividualEntrepreneur_SetsValidationError()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "123456789012", "Ivan", ClientType.IndividualEntrepreneur, now, now);
        var founder = Person.CreateFounder(null, person.Id, "1234567890", "Founder", now, now);

        person.AddFounder(founder);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("Individual entrepreneur cannot have founders", person.Error!.Details!);
    }

    [Fact]
    public void AddFounder_DuplicateFounder_SetsValidationError()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founder = Person.CreateFounder(null, person.Id, "1111111111", "Founder", now, now);

        person.AddFounder(founder);
        person.AddFounder(founder);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("Duplicate founder", person.Error!.Details!);
    }

    [Fact]
    public void Create_LegalEntity_With10DigitsInn_IsValid()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);

        Assert.True(person.IsValid);
        Assert.Null(person.Error);
        Assert.Equal("1234567890", person.INN);
    }

    [Fact]
    public void Create_LegalEntity_With12DigitsInn_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "123456789012", "Company Ltd", ClientType.LegalEntity, now, now);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("Legal entity INN must be 10 digits", person.Error!.Details!);
    }

    [Fact]
    public void Create_Person_WithEmptyName_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "", ClientType.LegalEntity, now, now);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("Name has no value", person.Error!.Details!);
    }

    [Fact]
    public void CreateFounder_WithInvalidInn_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var founder = Person.CreateFounder(null, Guid.NewGuid(), "abcd123456", "Founder", now, now);

        Assert.False(founder.IsValid);
        Assert.NotNull(founder.Error);
        Assert.Contains("INN must contain only digits", founder.Error!.Details!);
    }

    [Fact]
    public void CreateFounder_WithEmptyFullName_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var founder = Person.CreateFounder(null, Guid.NewGuid(), "1234567890", "", now, now);

        Assert.False(founder.IsValid);
        Assert.NotNull(founder.Error);
        Assert.Contains("Name has no value", founder.Error!.Details!);
    }

    [Fact]
    public void AddFounder_ToLegalEntity_AddsFounderSuccessfully()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founder = Person.CreateFounder(null, person.Id, "1111111111", "Founder", now, now);

        person.AddFounder(founder);

        Assert.True(person.IsValid);
        Assert.Null(person.Error);
        Assert.Single(person.Founders);
        Assert.Equal("1111111111", person.Founders.Single().INN);
    }

    [Fact]
    public void RemoveFounder_RemovesFounderFromPerson()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founder = Person.CreateFounder(null, person.Id, "1111111111", "Founder", now, now);

        person.AddFounder(founder);
        person.RemoveFounder("1111111111");

        Assert.True(person.IsValid);
        Assert.Null(person.Error);
        Assert.Empty(person.Founders);
    }

    [Fact]
    public void ChangeName_WithValidName_UpdatesName()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);

        person.ChangeName("New Company Name");

        Assert.True(person.IsValid);
        Assert.Null(person.Error);
        Assert.Equal("New Company Name", person.Name);
    }

    [Fact]
    public void ChangeName_WithInvalidName_SetsValidationError()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);

        person.ChangeName(" ");

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("Name has no value", person.Error!.Details!);
        Assert.Equal("Company Ltd", person.Name);
    }

    [Fact]
    public void ChangeFounderFullName_WithValidName_UpdatesName()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founder = Person.CreateFounder(null, person.Id, "1111111111", "Founder", now, now);

        person.ChangeFounderFullName(founder, "New Founder Name");

        Assert.True(founder.IsValid);
        Assert.Null(founder.Error);
        Assert.Equal("New Founder Name", founder.FullName);
    }

    [Fact]
    public void ChangeFounderFullName_WithInvalidName_SetsValidationError()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founder = Person.CreateFounder(null, person.Id, "1111111111", "Founder", now, now);

        person.ChangeFounderFullName(founder, " ");

        Assert.False(founder.IsValid);
        Assert.NotNull(founder.Error);
        Assert.Contains("Name has no value", founder.Error!.Details!);
        Assert.Equal("Founder", founder.FullName);
    }

    [Fact]
    public void Create_IndividualEntrepreneur_WithNonDigitInn_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "12345abc9012", "Ivan Ivanov", ClientType.IndividualEntrepreneur, now, now);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("INN must contain only digits", person.Error!.Details!);
    }

    [Fact]
    public void Create_LegalEntity_WithEmptyInn_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, string.Empty, "Company Ltd", ClientType.LegalEntity, now, now);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("INN cannot be empty", person.Error!.Details!);
    }

    [Fact]
    public void CreateFounder_WithInvalidLengthInn_IsInvalid()
    {
        var now = DateTime.UtcNow;
        var founder = Person.CreateFounder(null, Guid.NewGuid(), "123456789", "Founder", now, now);

        Assert.False(founder.IsValid);
        Assert.NotNull(founder.Error);
        Assert.Contains("INN must be either 10 or 12 digits", founder.Error!.Details!);
    }

    [Fact]
    public void AddFounder_WithInvalidFounderInn_SetsValidationError()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founder = Person.CreateFounder(null, person.Id, "12345abc90", "Founder", now, now);

        person.AddFounder(founder);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("INN must contain only digits", person.Error!.Details!);
        Assert.Empty(person.Founders);
    }

    [Fact]
    public void RemoveFounder_NonexistentFounder_DoesNotChangeFounders()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founder = Person.CreateFounder(null, person.Id, "1111111111", "Founder", now, now);

        person.AddFounder(founder);
        person.RemoveFounder("2222222222");

        Assert.True(person.IsValid);
        Assert.Null(person.Error);
        Assert.Single(person.Founders);
    }

    [Fact]
    public void AddFounder_DifferentFounderSameInn_SetsValidationError()
    {
        var now = DateTime.UtcNow;
        var person = Person.Create(null, "1234567890", "Company Ltd", ClientType.LegalEntity, now, now);
        var founderOne = Person.CreateFounder(null, person.Id, "1111111111", "Founder One", now, now);
        var founderTwo = Person.CreateFounder(null, person.Id, "1111111111", "Founder Two", now, now);

        person.AddFounder(founderOne);
        person.AddFounder(founderTwo);

        Assert.False(person.IsValid);
        Assert.NotNull(person.Error);
        Assert.Contains("Duplicate founder", person.Error!.Details!);
        Assert.Single(person.Founders);
    }
}
