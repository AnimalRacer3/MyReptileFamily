using System.Text.RegularExpressions;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Enum;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.Abstractions;
using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL;

namespace MyReptileFamilyAPI.Handlers;

public class Register(IMRFRepository Repo, IEmailService EmailService, EmailSettings EmailSettings) : IRegister
{
    /// <summary>
    ///     Adds a user into the database
    /// </summary>
    /// <param name="Owner">User to be added into the database.</param>
    /// <param name="CancellationToken"></param>
    /// <returns></returns>
    public async Task<IResult> RegisterUserAsync(RegisterOwner Owner, CancellationToken CancellationToken)
    {
        if (!Owner.BasicIsValid(out RegisterUserResult result)) return Results.BadRequest(result);

        if (!EmailService.IsDomainValid(Owner.Email)) return Results.BadRequest(RegisterUserResult.InvalidEmail);
        
        // Connecting to DB to finish validation
        await using (IMySQLConnection sqlConn = Repo.CreateMySQLConnection())
        {
            await sqlConn.OpenAsync(CancellationToken);

            IEnumerable<string> prohibitedWords = await Repo.QueryAsync(new GetBadWordsQuery(), sqlConn);
            // Check if the username contains any prohibited words
            if (prohibitedWords.Any(Word => Regex.IsMatch(Owner.Username, $@"\b{Regex.Escape(Word)}\b", RegexOptions.IgnoreCase)))
                return Results.BadRequest(RegisterUserResult.InvalidUsername);
            if (await Repo.ExecuteScalarAsync(new CheckIfUserExistsQuery(Owner.Username), sqlConn))
                return Results.Conflict("A user with this username already exists");
            if (await Repo.ExecuteScalarAsync(new CheckIfEmailExistsQuery(Owner.Email), sqlConn))
                return Results.Conflict("A user with this email already exists");

            await Repo.ExecuteAsync(new CreateOwnerSQL(Owner), sqlConn);

            await sqlConn.CloseAsync();
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
            
            Best,
            MyReptileFamily Team
            """ : EmailSettings.BaseRegisterEmail.PlainTextContent
        };

        await EmailService.SendEmailAsync(registerEmail, CancellationToken);

        return Results.Ok(result);
    }
}