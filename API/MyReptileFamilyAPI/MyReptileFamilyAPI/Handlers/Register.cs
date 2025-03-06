using System.Text.RegularExpressions;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Enum;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.Services;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.Abstractions;
using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL;

namespace MyReptileFamilyAPI.Handlers;

public class Register(IMRFRepository Repo, IRegisterService RegisterService, IEmailService EmailService) : IRegister
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
        await using IMySQLConnection sqlConn = Repo.CreateMySQLConnection();
        await sqlConn.OpenAsync(CancellationToken);
        IResult validationResult = await RegisterService.ValidateOwnerWithDatabaseAsync(Owner, sqlConn, CancellationToken);
        if (validationResult != Results.Ok()) return validationResult;

        await RegisterService.CreateValidationAndWelcomeEmailAsync(Owner, sqlConn, CancellationToken);
        await sqlConn.CloseAsync();

        return Results.Ok(result);
    }

    public async Task<IResult> AuthUserAsync(string Username, string Token, CancellationToken CancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Token)) return Results.BadRequest(Token);

        // Connecting to DB to finish validation
        await using IMySQLConnection sqlConn = Repo.CreateMySQLConnection();
        await sqlConn.OpenAsync(CancellationToken);
        string? _hashedToken = await Repo.QueryFirstOrDefaultAsync(new GetRegisterToken(Username), sqlConn);
        if (_hashedToken == null) return Results.BadRequest($"{Username} needs to re-validate their email token.");
        if (!BCrypt.Net.BCrypt.Verify(Token, _hashedToken)) return Results.BadRequest("Invalid token.");
        await Repo.ExecuteAsync(new OwnerEmailValidated(Username), sqlConn);
        await sqlConn.CloseAsync();
        return Results.Ok($"{Username}'s email has been verified!");
    }
}