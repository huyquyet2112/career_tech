using Microsoft.Extensions.Localization;

namespace CareerTech.Shared;

public class Utils
{
    public static string GetLocalizedMessage(string key, IStringLocalizer localizer)
    {
        return localizer[key].Value;
    }
}
