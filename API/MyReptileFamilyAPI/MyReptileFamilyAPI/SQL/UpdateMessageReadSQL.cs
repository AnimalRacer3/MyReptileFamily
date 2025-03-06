using MyReptileFamilyAPI.Record;
using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

namespace MyReptileFamilyAPI.SQL;

public class UpdateMessageReadSQL(MessageDTO Message) : IDapperSQL
{
    public string SQL =>
        """
        UPDATE OwnerMessages om
        SET om.IsRead = @IsRead
        WHERE om.MessageID = @MessageId
        """;

    public DapperParameter DapperParameter =>
        new([
            Message.IsRead.ToSqlParameter("@IsRead"),
            Message.MessageId.ToSqlParameter("@MessageId")
        ]);
}