using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;
using MySqlConnector;

namespace MyReptileFamilyAPI.SQL;

public class CheckIfEmailExistsQuery(string Email) : IDapperQuery<bool>
{
    public string SQL => """
                         SELECT EXISTS (
                             SELECT 1 
                             FROM Owner 
                             WHERE Email = @username
                             );
                         """;
    public List<MySqlParameter> Parameters => [Email.ToSqlParameter("@email", 64)];
}