using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Application.Interfaces;

public interface INotificationService
{
    Task CountNotificationService(Guid userId);
    Task PushNotificationFriendService(ViewUserAddedFriendDTO viewUserAddedFriendDto, AddNotificationFriendDTO addNotificationFriendDto);
    Task<Result<object>> GetTwoNotificationFriendService(Guid userId);
}
