using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;
using MySqlConnector;

namespace MyReptileFamilyAPI.SQL;

public class GetPasswordHashQuery(string? Username, string? Email) : IDapperQuery<string>
{
    public string SQL => """
                         SELECT 
                             PasswordHash
                         FROM Owners o 
                         WHERE (Username = @username OR Email = @email)
                         """;

    public List<MySqlParameter> Parameters =>
    [
        Username.ToSqlParameter("@username", 16),
        Email.ToSqlParameter("@email", 64)
    ];
}