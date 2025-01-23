using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;
using MySqlConnector;

namespace MyReptileFamilyAPI.SQL;

public class GetRegisterToken(string Username) : IDapperQuery<string>
{
    public string SQL => """
                         SELECT TOP (1)
                            HashedToken
                         FROM OwnerToken ot
                         INNER JOIN Owners o ON o.OwnerID = ot.OwnerID
                         WHERE ot.ForRegistration = TRUE 
                         	AND ot.ExpDateTime > CURRENT_TIMESTAMP()
                         	AND o.Username = @username
                         """;

    public DapperParameter DapperParameter => new(Username.ToSqlParameter("@username", 16));
}