using MyReptileFamilyAPI.Enum;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.SQL;
using System.Text.RegularExpressions;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.Abstractions;

namespace MyReptileFamilyAPI.Services;

public class RegisterService(IMRFRepository Repo, IEmailService EmailService, EmailSettings EmailSettings) : IRegisterService
{
    public async Task<IResult> ValidateOwnerWithDatabaseAsync(RegisterOwner Owner, IMySQLConnection SqlConn, CancellationToken CancellationToken)
    {
        IEnumerable<string> prohibitedWords = await Repo.QueryAsync(new GetBadWordsQuery(), SqlConn);
        // Check if the username contains any prohibited words
        if (prohibitedWords.Any(Word => Regex.IsMatch(Owner.Username, $@"\b{Regex.Escape(Word)}\b", RegexOptions.IgnoreCase)))
            return Results.BadRequest(RegisterUserResult.InvalidUsername);
        if (await Repo.ExecuteScalarAsync(new CheckIfUserExistsQuery(Owner.Username), SqlConn))
            return Results.Conflict("A user with this username already exists");
        if (await Repo.ExecuteScalarAsync(new CheckIfEmailExistsQuery(Owner.Email), SqlConn))
            return Results.Conflict("A user with this email already exists");

        await Repo.ExecuteAsync(new CreateOwnerSQL(Owner), SqlConn);

        return Results.Ok();
    }

    public async Task CreateValidationAndWelcomeEmailAsync(RegisterOwner Owner, IMySQLConnection SqlConn, CancellationToken CancellationToken)
    {
        string? _hashedToken = await Repo.QueryFirstOrDefaultAsync(new GetRegisterToken(Owner.Username), SqlConn);
        string _token = Guid.NewGuid().ToString();
        if (_hashedToken == null)
        {
            _hashedToken = BCrypt.Net.BCrypt.HashPassword(_token);

            await Repo.ExecuteAsync(new AddRegistrationToken(_hashedToken, Owner.Username), SqlConn);
        }
        else
        {
            _hashedToken = BCrypt.Net.BCrypt.HashPassword(_token);

            await Repo.ExecuteAsync(new UpdateRegistrationToken(_hashedToken, Owner.Username), SqlConn);
        }
        
        Email registerEmail = EmailSettings.BaseRegisterEmail with
        {
            To = [Owner.Email],
            Subject = string.IsNullOrWhiteSpace(EmailSettings.BaseRegisterEmail.Subject) ? "Welcome to Your New Reptile Family!" : EmailSettings.BaseRegisterEmail.Subject,
            PlainTextContent = string.IsNullOrWhiteSpace(EmailSettings.BaseRegisterEmail.PlainTextContent) ?
                $"""
                 Hi {Owner.Username},

                 Welcome to MyReptileFamily! We're so excited to have you as part of our growing community of 
                 reptile enthusiasts. Whether you're a Herpetologist or a beginner, you're in the right place
                 to share, learn, and grow your reptile family.

                 If you have any questions or need help getting started, feel free to reach out to our support team at:
                 support@myreptilefamily.com
                 Thank you for joining MyReptileFamily - we can't wait to see the amazing things you'll share with us!
                 
                 Please go to www.MyReptileFamily.com/auth?username={Owner.Username}&token={_token} To validate your email.

                 Best,
                 MyReptileFamily Team
                 """ : EmailSettings.BaseRegisterEmail.PlainTextContent
        };

        await EmailService.SendEmailAsync(registerEmail, CancellationToken);
    }
}