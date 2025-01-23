using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

namespace MyReptileFamilyAPI.SQL;

public class OwnerEmailValidated(string Username) : IDapperSQL
{
    public string SQL =>
        """
        UPDATE Owners
        SET EmailVerified = 1
        WHERE Username = @username;
        """;

    public DapperParameter DapperParameter => new(Username.ToSqlParameter("@Username", 16));
}