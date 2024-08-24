using MessengerServer.Src.Domain.Enum;

namespace MessengerServer.Src.Contracts.DTOs.FriendshipDTOs;

public class GetFriendshipDTO
{
    public Guid Id { get; set; }
    public Guid UserInitId { get; set; }
    public Guid UserReceiveId { get; set; }
    public bool IsUserInit { get; set; }
    public FriendshipStatusEnum Status { get; set; }
}
