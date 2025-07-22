using System.Globalization;
using System.Text.RegularExpressions;

namespace CareerTech.Common.Utils;

public class Helper
{
    public static DateTime ConvertStringToDateTime(string dateString, string format = "yyyy/MM/dd")
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        bool success = DateTime.TryParseExact(dateString, format, provider, DateTimeStyles.None, out DateTime dateTime);
        if (success)
        {
            return dateTime;
        }
        else
        {
            throw new Exception("Datetime invalid");
        }
    }

    public static string GenerateUUID()
    {
        Guid newUuid = Guid.NewGuid();

        return newUuid.ToString();
    }

    public static bool ICorrectPasswordFormat(string password)
    {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        Regex regex = new(pattern);
        return regex.IsMatch(password);
    }

    public static string Encode(string permission)
    {
        const uint fnvPrime = 123;
        const uint offSetBasis = 2315;
        uint hash = offSetBasis;

        foreach (char c in permission)
        {
            hash ^= c;
            hash *= fnvPrime;
        }

        return $"{hash}";
    }

    public static List<string> ConvertStringToUnit(List<string> permission)
    {
        return permission.Select(Encode).ToList();
    }

    public static bool HasPermission(IEnumerable<string> permissionUser, string controller, string action)
    {
        var permission = Encode($"{controller}.{action}");

        return permissionUser.Any(us => us == permission);
    }
}
