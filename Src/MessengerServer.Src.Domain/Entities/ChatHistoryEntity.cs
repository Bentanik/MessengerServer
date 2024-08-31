namespace MessengerServer.Src.Domain.Entities;

public class ChatHistoryEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ChatPartnerId { get; set; }
    public DateTime LastMessageTimestamp { get; set; }
    public UserEntity User { get; set; }
    public UserEntity ChatPartner { get; set; }
    public bool Read { get; set; }
}
