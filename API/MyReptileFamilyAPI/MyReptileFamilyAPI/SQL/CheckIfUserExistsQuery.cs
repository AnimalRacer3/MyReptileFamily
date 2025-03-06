using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

namespace MyReptileFamilyAPI.SQL;

public class CheckIfUserExistsQuery(string Username) : IDapperQuery<bool>
{
    public string SQL => """
                         SELECT EXISTS (
                             SELECT 1 
                             FROM Owners
                             WHERE Username = @username
                             );
                         """;

    public DapperParameter DapperParameter => new([Username.ToSqlParameter("@username", 16)]);
}