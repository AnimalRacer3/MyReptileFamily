using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

namespace MyReptileFamilyAPI.SQL;

public class UpdateRegistrationToken(string HashedToken, string Username) : IDapperSQL
{
    public string SQL => """
                         UPDATE OwnerToken
                         SET HashedToken = @hashToken,
                         ExpDateTime = DATE_ADD(NOW(), INTERVAL 24 HOUR),
                         WHERE OwnerID = (SELECT OwnerID FROM Owners WHERE Username = @username) AND ForRegistration = TRUE;
                         """;

    public DapperParameter DapperParameter => new(
        [ HashedToken.ToSqlParameter("@hashToken", 64),
            Username.ToSqlParameter("@username", 16)]);
}