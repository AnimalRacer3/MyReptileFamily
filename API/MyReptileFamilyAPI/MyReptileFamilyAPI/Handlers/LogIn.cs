using BCrypt.Net;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Enum;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.SQL;
using BCryptNet = BCrypt.Net.BCrypt;

namespace MyReptileFamilyAPI.Handlers;

public class LogIn(IMRFRepository Repo) : ILogIn
{
    public async Task<IResult> UserLogIn(Owner User, CancellationToken Cancellation)
    {
        if (!User.BasicIsValid(out RegisterUserResult reason)) return Results.BadRequest(reason.ToString());

        if (!await IsPasswordCorrect(User, Cancellation)) return Results.Unauthorized();

        return Results.Ok(true);
    }

    private async Task<bool> IsPasswordCorrect(Owner User, CancellationToken Cancellation)
    {
        await using IMySQLConnection sqlConn = Repo.CreateMySQLConnection();
        await sqlConn.OpenAsync(Cancellation);
        string? passwordHash =
            await Repo.QueryFirstOrDefaultAsync(new GetPasswordHashQuery(User.Username, User.Email), sqlConn);
        return BCryptNet.EnhancedVerify(User.Password, passwordHash, HashType.SHA512);
    }
}