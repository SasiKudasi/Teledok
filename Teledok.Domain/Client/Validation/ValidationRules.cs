using Teledok.Contracts.Shared.Enums;

namespace Teledok.Domain.Client.Validation;

internal static class ValidationRules
{
    internal static string? CheckInn(string value, ClientType clientType)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "INN cannot be empty.";

        if (!value.All(char.IsDigit))
            return "INN must contain only digits.";

        if (value.Length != 10 && value.Length != 12)
            return "INN must be either 10 or 12 digits.";

        return clientType switch
        {
            ClientType.LegalEntity when value.Length != 10 =>
                "Legal entity INN must be 10 digits.",

            ClientType.IndividualEntrepreneur when value.Length != 12 =>
                "Individual entrepreneur INN must be 12 digits.",

            _ => null
        };
    }

    internal static string? CanAddFounder(ClientType clientType, List<Founder> founders, string founderInn)
    {
        if (clientType is ClientType.IndividualEntrepreneur)
        {
            return "Individual entrepreneur cannot have founders";
        }
        if (founders.Any(x => x.INN == founderInn))
        {
            return "Duplicate founder";
        }

        return CheckInn(founderInn);
    }


    internal static string? CheckName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            return "Name has no value";
        }
        return null;
    }
}
