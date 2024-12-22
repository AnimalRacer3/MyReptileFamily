using MyReptileFamilyAPI.Enum;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.SQL;
using BCryptNet = BCrypt.Net.BCrypt;

namespace MyReptileFamilyAPI.Models;

public record Owner(
    string? Username,
    string? Email,
    string Password)
{
    public async Task<bool> PasswordValidation(IMySQLConnection SqlConn, IMRFRepository Repo)
    {
        string? _passwordHash = await Repo.QueryFirstOrDefaultAsync(new GetPasswordHashQuery(Username, Email), SqlConn);
        return BCryptNet.EnhancedVerify(Password, _passwordHash);
    }
    public bool BasicIsValid(out RegisterUserResult ReasonOfResult)
    {
        // Check if password is valid and within a character limit
        if (string.IsNullOrWhiteSpace(Password) || !ComponentRegularExpressions.PasswordRegex().IsMatch(Password))
        {
            ReasonOfResult = RegisterUserResult.InvalidPassword;
            return false;
        }
        // Check if the username is within a character limit and only contains letter numbers and '_'
        if (string.IsNullOrWhiteSpace(Username) || !ComponentRegularExpressions.UsernameRegex().IsMatch(Username) 
            || string.IsNullOrWhiteSpace(Email) || !ComponentRegularExpressions.EmailRegex().IsMatch(Email))
        {
            ReasonOfResult = RegisterUserResult.InvalidUsername;
            return false;
        }

        ReasonOfResult = RegisterUserResult.Success;
        return true;
    }
}