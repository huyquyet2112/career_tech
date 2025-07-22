using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace CareerTech.Model.Enums;

/// <summary>
/// EEXperienceLevel.
/// </summary>
public enum EExperienceYear
{
    NotRequired,
    LessThanOneYear,
    OneToTwoYears,
    TwoToThreeYears,
    ThreeToFiveYears,
    MoreThanFiveYears,
}

public static class ConvertLevel
{
    public static string ConvertExperienceLevel(this EExperienceYear level)
    {
        return level switch
        {
            EExperienceYear.NotRequired => "Not required",
            EExperienceYear.LessThanOneYear => "1 year",
            EExperienceYear.OneToTwoYears => "1 - 2 years",
            EExperienceYear.TwoToThreeYears => "2 - 3 years",
            EExperienceYear.ThreeToFiveYears => "3 - 5 years",
            EExperienceYear.MoreThanFiveYears => "More than 5 years",
            _ => throw new NotImplementedException()
        };
    }
}

