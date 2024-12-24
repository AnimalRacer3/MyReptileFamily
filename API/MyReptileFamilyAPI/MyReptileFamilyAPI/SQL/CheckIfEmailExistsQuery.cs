using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

namespace MyReptileFamilyAPI.SQL;

public class CheckIfEmailExistsQuery(string Email) : IDapperQuery<bool>
{
    public string SQL => """
                         SELECT EXISTS (
                             SELECT 1 
                             FROM Owners
                             WHERE Email = @email
                             );
                         """;

    public DapperParameter DapperParameter => new(Email.ToSqlParameter("@email", 64));
}