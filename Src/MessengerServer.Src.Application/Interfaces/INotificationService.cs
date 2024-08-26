using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Application.Interfaces;

public interface INotificationService
{
    Task CountNotification(Guid userId);
    Task PushNotificationFriend(ViewUserAddedFriendDTO viewUserAddedFriendDto, AddNotificationFriendDTO addNotificationFriendDto);
}
