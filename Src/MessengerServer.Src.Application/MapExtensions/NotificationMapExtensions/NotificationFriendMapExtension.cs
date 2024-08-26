using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.MapExtensions.NotificationMapExtensions;

public static class NotificationFriendMapExtension
{
    public static NotificationAddFriendEntitiy ToNotificationAddFriend(this AddNotificationFriendDTO dto)
    {
        return new NotificationAddFriendEntitiy()
        {
            FromUserId = dto.FromUserId,
            ToUserId = dto.ToUserId,
            NotificationType = dto.NotificationType,
        };
    }
}
