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

public class Register(IRegisterService RegisterService, IEmailService EmailService) : IRegister
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

        IResult validationResult = await RegisterService.ValidateOwnerWithDatabaseAsync(Owner, CancellationToken);
        if (validationResult != Results.Ok()) return validationResult;

        await RegisterService.CreateValidationAndWelcomeEmailAsync(Owner, CancellationToken);

        return Results.Ok(result);
    }
}