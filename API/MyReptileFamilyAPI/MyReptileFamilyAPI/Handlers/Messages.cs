using Microsoft.AspNetCore.Mvc.Infrastructure;
using MyReptileFamilyAPI.Record;
using MyReptileFamilyAPI.SQL;
using MyReptileFamilyLibrary.SQL;

namespace MyReptileFamilyAPI.Handlers;

public class Messages(IMRFRepository Repo) : IMessages
{
    public async Task<IResult> SendAsync(MessageDTO Message, CancellationToken CancellationToken)
    {
        await using IMySQLConnection _sqlConn = Repo.CreateMySQLConnection();
        await _sqlConn.OpenAsync(CancellationToken);
        if (Message.MessageId.HasValue)
        {
            await Repo.ExecuteAsync(new AddMessageSQL(Message), _sqlConn);
        }
        else
        {
            await Repo.ExecuteAsync(new UpdateMessageReadSQL(Message), _sqlConn);
        }
        return Results.Ok();
    }

    public async Task<IResult> GetMessages(long SenderId, long ReceiverId, int Count, CancellationToken CancellationToken)
    {
        await using IMySQLConnection _sqlConn = Repo.CreateMySQLConnection();
        await _sqlConn.OpenAsync(CancellationToken);

        List<MessageDTO> messages = await Repo.QueryAsync(new GetMessagesQuery(SenderId, ReceiverId, Count), _sqlConn);
        return Results.Ok(messages);
    }
}