using MyReptileFamilyAPI.Enum;
using MyReptileFamilyAPI.Models;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;
using MySqlConnector;

namespace MyReptileFamilyAPI.SQL;

public class CreateOwnerSQL(RegisterOwner Owner) : IDapperSQL
{
    public string SQL => """
                         INSERT INTO Owners (Username, Email, PasswordHash, Visibility, Birthday, CurrentIntent, Country, PostalCode, Street1, Street2, City, State)
                         VALUE (@username, @email, @passwordHash, @visibility, @birthday, @currentIntent, @country, @postalCode, @street1, @street2, @city, @state)
                         """;

    public MySqlParameter[] Parameters =>
    [
        Owner.Username.ToSqlParameter("@username", 16),
        Owner.Email.ToSqlParameter("@email", 64),
        Owner.PasswordHash.ToSqlParameter("@passwordHash", 255),
        Owner.Visibility.ToSqlParameter("@visibility"),
        Owner.Birthday.ToSqlParameter("@birthday"),
        Owner.Intent.ToSqlParameter("@currentIntent"),
        Owner.Country.ToSqlParameter("@country", 64),
        Owner.PostalCode.ToSqlParameter("@postalCode", 10),
        Owner.Street1.ToSqlParameter("@street1", 64),
        Owner.Street2.ToSqlParameter("@street2", 64),
        Owner.City.ToSqlParameter("@city", 128),
        Owner.State.ToSqlParameter("@state", 64)
    ];
}