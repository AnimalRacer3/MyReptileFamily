using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;
using MySqlConnector;

namespace MyReptileFamilyAPI.SQL;

public class AddRegistrationToken(string HashedToken, string Username) : IDapperSQL
{
    public string SQL => """
                         INSERT INTO OwnerToken (HashedToken, OwnerID, ExpDateTime, ForRegistration)
                         VALUES (@hashToken, 
                                 (SELECT OwnerID FROM Owners WHERE Username = @username), 
                                 DATE_ADD(NOW(), INTERVAL 24 HOUR), 
                                 TRUE);
                         """;

    public DapperParameter DapperParameter => 
        new([
            HashedToken.ToSqlParameter("@hashToken", 64), 
            Username.ToSqlParameter("@username", 16)]
            );
}