using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.SQL;
using MySqlX.XDevAPI.Common;

namespace MyReptileFamilyAPI.Handlers;

public class LogIn(DbSettings DbSettings, IMRFRepository Repo) : ILogIn
{
    public async Task<IResult> UserLogIn(Owner User, CancellationToken Cancellation)
    {
        if (!User.BasicIsValid(out var reason)) return Results.BadRequest(reason.ToString());

        await using (IMySQLConnection sqlConn = Repo.CreateMySQLConnection())
        {
            await sqlConn.OpenAsync(Cancellation);
            if (await User.PasswordValidation(sqlConn, Repo)) return Results.Unauthorized();
            await sqlConn.CloseAsync();
        }

        return Results.Ok(true);
    }
}