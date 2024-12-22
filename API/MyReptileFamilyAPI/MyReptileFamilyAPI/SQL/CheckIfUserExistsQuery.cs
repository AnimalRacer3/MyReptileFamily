using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;
using MySqlConnector;

namespace MyReptileFamilyAPI.SQL;

public class CheckIfUserExistsQuery(string Username) : IDapperQuery<bool>
{
    public string SQL => """
                         SELECT EXISTS (
                             SELECT 1 
                             FROM Owner 
                             WHERE Username = @username
                             );
                         """;
    public List<MySqlParameter> Parameters => [Username.ToSqlParameter("@username", 16),];
}