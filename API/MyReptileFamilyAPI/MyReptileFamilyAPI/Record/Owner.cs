using MyReptileFamilyAPI.Enum;

namespace MyReptileFamilyAPI.Models;

public record Owner(
    string? Username,
    string? Email,
    string Password)
{
    public bool BasicIsValid(out RegisterUserResult ReasonOfResult)
    {
        // Check if password is valid and within a character limit
        if (string.IsNullOrWhiteSpace(Password) || !ComponentRegularExpressions.PasswordRegex().IsMatch(Password))
        {
            ReasonOfResult = RegisterUserResult.InvalidPassword;
            return false;
        }

        // Check if the username is within a character limit and only contains letter numbers and '_'
        if (string.IsNullOrWhiteSpace(Username) || !ComponentRegularExpressions.UsernameRegex().IsMatch(Username))
            if (string.IsNullOrWhiteSpace(Email) || !ComponentRegularExpressions.EmailRegex().IsMatch(Email))
            {
                ReasonOfResult = RegisterUserResult.InvalidUsername;
                return false;
            }

        ReasonOfResult = RegisterUserResult.Success;
        return true;
    }
}