namespace MessengerServer.Src.Contracts.DTOs.MessageDTOs;

public class CreateMessageDTO
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string? Content { get; set; }
}
