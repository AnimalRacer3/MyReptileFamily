using System.Text.RegularExpressions;

namespace MyReptileFamilyAPI;

internal static partial class ComponentRegularExpressions
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public static partial Regex EmailRegex();

    [GeneratedRegex(
        @"^(?=(?:.*[A-Z]){2,})(?=(?:.*[a-z]){2,})(?=(?:.*\d){1,})(?=(?:.*[!@#$%^&*()\-_=+{};:,<.>?]){1,})(?!.*(.)\1{2})([A-Za-z0-9!@#$%^&*()\-_=+{};:,<.>?]{8,32})$")]
    public static partial Regex PasswordRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9_]{3,16}$")]
    public static partial Regex UsernameRegex();
}