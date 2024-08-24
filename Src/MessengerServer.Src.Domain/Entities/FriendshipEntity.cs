using MessengerServer.Src.Domain.Enum;

namespace MessengerServer.Src.Domain.Entities;

public class FriendshipEntity : BaseEntity
{
    public Guid UserInitId { get; set; }
    public Guid UserReceiveId { get; set; }
    public FriendshipStatusEnum Status { get; set; }
    public UserEntity UserInitiated { get; set; }
    public UserEntity UserReceived { get; set; }
}
