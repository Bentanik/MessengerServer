using MessengerServer.Src.Domain.Enum;

namespace MessengerServer.Src.Contracts.DTOs.NotificationDTOs;

public class AddNotificationFriendDTO
{
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public NotificationTypeEnum NotificationType { get; set; }
}


