namespace MessengerServer.Src.Domain.Entities;

public class MessageEntity : BaseEntity
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; }
    public UserEntity Sender { get; set; }
    public UserEntity Receiver { get; set; }
}

