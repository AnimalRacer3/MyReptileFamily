using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.SQL;
using MySqlX.XDevAPI.Common;

namespace MyReptileFamilyAPI.Handlers;

public class LogIn(DbSettings DbSettings, OwnerSettings OSettings, IMRFRepository Repo) : ILogIn
{
    public async Task<IResult> UserLogIn(Owner User, CancellationToken Cancellation)
    {
        if (!User.BasicIsValid(OSettings.UsernameRange, OSettings.PasswordRange, out var reason)) return Results.BadRequest("Username is invalid");

        await using (IMySQLConnection sqlConn = Repo.CreateMySQLConnection(DbSettings.ConnectionString))
        {
            await sqlConn.OpenAsync(Cancellation);
            if (await User.PasswordValidation(sqlConn, Repo)) return Results.Unauthorized();
            await sqlConn.CloseAsync();
        }

        return Results.Ok(true);
    }
}