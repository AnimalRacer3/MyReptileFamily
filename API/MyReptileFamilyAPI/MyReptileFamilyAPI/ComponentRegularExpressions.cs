using System.Text.RegularExpressions;

namespace MyReptileFamilyAPI;

internal static partial class ComponentRegularExpressions
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public static partial Regex EmailRegex();

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]$")]
    public static partial Regex PasswordRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9_]+$")]
    public static partial Regex UsernameRegex();
}