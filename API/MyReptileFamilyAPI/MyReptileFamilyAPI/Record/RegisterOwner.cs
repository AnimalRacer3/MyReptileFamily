using BCrypt.Net;
using BCryptNet = BCrypt.Net.BCrypt;
using MyReptileFamilyAPI.Enum;

namespace MyReptileFamilyAPI.Models;

public record RegisterOwner(
    string Username,
    string Email,
    string Password,
    int VisibilityNo,
    int IntentNo,
    string? PostalCode,
    string? Street1,
    string? Street2,
    string? City,
    string? State,
    string? Country,
    DateTime? Birthday,
    string? PathToProfileImage)
{
    public VisibilityEnum Visibility => (VisibilityEnum)VisibilityNo;
    public IntentEnum Intent => (IntentEnum)IntentNo;
    // Check if any address field is provided
    private bool isAnyAddressFieldProvided => !string.IsNullOrWhiteSpace(PostalCode) ||
                                     !string.IsNullOrWhiteSpace(Street1) ||
                                     !string.IsNullOrWhiteSpace(Street2) ||
                                     !string.IsNullOrWhiteSpace(City) ||
                                     !string.IsNullOrWhiteSpace(State) ||
                                     !string.IsNullOrWhiteSpace(Country);

    private bool isAllAddressFieldsProvided => !string.IsNullOrWhiteSpace(PostalCode) &&
                                              (!string.IsNullOrWhiteSpace(Street1) || !string.IsNullOrWhiteSpace(Street2)) &&
                                              !string.IsNullOrWhiteSpace(City) &&
                                              !string.IsNullOrWhiteSpace(State) &&
                                              !string.IsNullOrWhiteSpace(Country);

    public string PasswordHash => BCryptNet.EnhancedHashPassword(Password, HashType.SHA512, 15);
    
    public bool BasicIsValid(out RegisterUserResult ReasonOfResult)
    {
        // Check for valid email with regex
        if (string.IsNullOrWhiteSpace(Email) || !ComponentRegularExpressions.EmailRegex().IsMatch(Email))
        {
            ReasonOfResult = RegisterUserResult.InvalidEmail;
            return false;
        }
        // Check if password is valid and within a character limit
        if (string.IsNullOrWhiteSpace(Password) || !ComponentRegularExpressions.PasswordRegex().IsMatch(Password))
        {
            ReasonOfResult = RegisterUserResult.InvalidPassword;
            return false;
        }
        // Check if the username is within a character limit and only contains letter numbers and '_'
        if (string.IsNullOrWhiteSpace(Username) || !ComponentRegularExpressions.UsernameRegex().IsMatch(Username))
        {
            ReasonOfResult = RegisterUserResult.InvalidUsername;
            return false;
        }
        // Check if address fields are all provided if any are
        if (isAnyAddressFieldProvided && !isAllAddressFieldsProvided)
        {
            ReasonOfResult = RegisterUserResult.InvalidAddress;
            return false;
        }

        ReasonOfResult = RegisterUserResult.Success;
        return true;
    }
}