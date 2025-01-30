using MyReptileFamilyAPI.Record;

namespace MyReptileFamilyAPI.Handlers;

public interface IMessages
{
    Task<IResult> SendAsync(MessageDTO Message, CancellationToken CancellationToken);
    Task<IResult> GetMessages(long SenderId, long ReceiverId, int Count, CancellationToken CancellationToken);
}