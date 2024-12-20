using System.Text.RegularExpressions;
using MyReptileFamilyAPI.AppSettings;
using MyReptileFamilyAPI.Enum;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.SQL;
using MySql.Data.MySqlClient;

namespace MyReptileFamilyAPI.Handlers;

public class Register(DbSettings DbSettings, OwnerSettings OSettings, IMRFRepository Repo) : IRegister
{
    /// <summary>
    /// Adds a user into the database
    /// </summary>
    /// <param name="Owner">User to be added into the database.</param>
    /// <param name="CancellationToken"></param>
    /// <returns></returns>
    public async Task<IResult> RegisterUserAsync(RegisterOwner Owner, CancellationToken CancellationToken)
    {
        if (!Owner.BasicIsValid(OSettings.UsernameRange, OSettings.PasswordRange, out RegisterUserResult result)) return Results.BadRequest(result);
        // Connecting to DB to finish validation
        await using (IMySQLConnection sqlConn = Repo.CreateMySQLConnection(DbSettings.ConnectionString))
        {
            await sqlConn.OpenAsync(CancellationToken);

            IEnumerable<string> prohibitedWords = await Repo.QueryAsync(new GetBadWordsQuery(), sqlConn);
            // Check if the username contains any prohibited words
            if (prohibitedWords.Any(word => Regex.IsMatch(Owner.Username, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase)))
                return Results.BadRequest(RegisterUserResult.InvalidUsername);
            if (!await Repo.ExecuteScalarAsync(new CheckIfUserExistsQuery(Owner.Username), sqlConn))
                return Results.Conflict("A user with this username already exists");
            if (!await Repo.ExecuteScalarAsync(new CheckIfEmailExistsQuery(Owner.Email), sqlConn))
                return Results.Conflict("A user with this email already exists");

            await Repo.ExecuteAsync(new CreateOwnerSQL(Owner), sqlConn);

            await sqlConn.CloseAsync();
        }

        return Results.Ok(result);
    }
}