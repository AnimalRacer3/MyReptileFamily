namespace MyReptileFamilyAPI.Record;

public record MessageDTO(
    long SenderId, 
    long ReceiverId, 
    string MessageText, 
    bool HasImage = false,
    bool IsRead = false,
    long? MessageId = null)
{
    //TODO: Add method to get image information if HasImage is true
}