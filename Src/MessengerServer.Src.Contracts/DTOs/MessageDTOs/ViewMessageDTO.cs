namespace MessengerServer.Src.Contracts.DTOs.MessageDTOs;

public class ViewMessageDTO
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; }
}
