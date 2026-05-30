using System.Diagnostics.Metrics;
using Teledok.Domain.Enums;

namespace Teledok.Domain.Client.Validation;

internal static class ValidationRules
{
    internal static string? CheckInn(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "INN cannot be empty.";


        if (!value.All(char.IsDigit))
            return "INN must contain only digits.";

        var valueLength = value.Length;
        if (valueLength != 10 || valueLength != 12)
            return "INN must be either 10 or 12 characters long.";

        return null;
    }

    internal static string? CanAddFounder(ClientType clientType, List<Founder> founders, string founderInn)
    {
        if (clientType is ClientType.IndividualEntrepreneur)
        {
            return "IP cannot have founders";
        }
        if (founders.Any(x => x.INN == founderInn))
        {
            return "Duplicate founder";
        }

        return CheckInn(founderInn);
    }


    internal static string? CheckName(string name)
    {
        if(!string.IsNullOrWhiteSpace(name))
        {
            return "Name has no value";
        }
        return null;
    }
}
