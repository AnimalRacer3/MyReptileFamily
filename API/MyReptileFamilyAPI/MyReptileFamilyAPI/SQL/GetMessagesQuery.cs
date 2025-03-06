using MyReptileFamilyAPI.Record;
using MyReptileFamilyLibrary.Records;
using MyReptileFamilyLibrary.SQL.Abstractions;
using MyReptileFamilyLibrary.SQL.MySqlParameterExtensions;

namespace MyReptileFamilyAPI.SQL;

public class GetMessagesQuery(long SenderId, long ReceiverId, int Count) : IDapperQuery<MessageDTO>
{
    public string SQL =>
        """
        SELECT
        	MessageID AS MessageId,
        	SenderID AS SenderId,
        	ReceiverID AS ReceiverId,
        	MessageText AS MessageText,
        	IsRead AS IsRead,
        	HasImage AS HasImage
        FROM OwnerMessages om
        WHERE ReceiverID = @ReceiverId
        	AND SenderID = @SenderId
        ORDER BY SentAt DESC
        LIMIT @Count
        """;

    public DapperParameter DapperParameter =>
        new([
            SenderId.ToSqlParameter("@SenderId"),
            ReceiverId.ToSqlParameter("@ReceiverId"),
            Count.ToSqlParameter("@Count")
        ]);
}