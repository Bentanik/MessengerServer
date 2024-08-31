namespace MessengerServer.Src.Contracts.DTOs.MessageDTOs;

public class CreateChatHistoryDTO
{
    public Guid UserId { get; set; }
    public Guid ChatPartnerId { get; set; }
    public bool Read { get; set; }
}
