using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Application.Repositories;

public interface IHubNotificationService
{
    Task SendNotificationFriendAsync(Guid userId, ViewUserAddedFriendDTO viewUserAddedFriendDTO);
    Task CountNotificationAsync(Guid userId, int countNotification);
}
