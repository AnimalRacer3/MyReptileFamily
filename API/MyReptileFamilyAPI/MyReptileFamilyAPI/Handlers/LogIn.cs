using BCrypt.Net;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.SQL;
using BCryptNet = BCrypt.Net.BCrypt;
using MyReptileFamilyLibrary.SQL;

namespace MyReptileFamilyAPI.Handlers;

public class LogIn(DbSettings DbSettings, IMRFRepository Repo) : ILogIn
{
    public async Task<IResult> UserLogIn(Owner User, CancellationToken Cancellation)
    {
        if (!User.BasicIsValid(out var reason)) return Results.BadRequest(reason.ToString());

        if (!await IsPasswordCorrect(User, Cancellation)) return Results.Unauthorized();
        
        return Results.Ok(true);
    }

    private async Task<bool> IsPasswordCorrect(Owner User, CancellationToken Cancellation)
    {
        await using IMySQLConnection sqlConn = Repo.CreateMySQLConnection();
        await sqlConn.OpenAsync(Cancellation);
        string? _passwordHash = await Repo.QueryFirstOrDefaultAsync(new GetPasswordHashQuery(User.Username, User.Email), sqlConn);
        return BCryptNet.EnhancedVerify(User.Password, _passwordHash, HashType.SHA512);
    }
}