using MyReptileFamilyAPI.Record;
using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

namespace MyReptileFamilyAPI.SQL;

public class AddMessageSQL(MessageDTO Message) : IDapperSQL
{
    public string SQL =>
        """
        INSERT INTO OwnerMessages(SenderId, ReceiverId, MessageText, HasImage)
        VALUES (@SenderId, @ReceiverId, @Content, @HasImage)
        """;

    public DapperParameter DapperParameter => 
        new ([
            Message.SenderId.ToSqlParameter("@SenderId"),
            Message.ReceiverId.ToSqlParameter("@ReceiverId"),
            Message.MessageText.ToSqlParameter("@Content"),
            Message.HasImage.ToSqlParameter("@HasImage")
        ]);
}