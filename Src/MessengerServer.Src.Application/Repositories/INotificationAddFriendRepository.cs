using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface INotificationAddFriendRepository : IRepositoryBase<NotificationAddFriendEntitiy>
{
    Task<int> CountNotificationAddedFriendByStatusAsync(Guid userId);
    Task<IEnumerable<ViewNotificationAddFriendDTO>> GetTwoInfoUserNotificationFriendAsync(Guid userId);
    Task<NotificationAddFriendEntitiy> GetNotificationAddFriendAsync(Guid fromUserId, Guid toUserId);
    Task<bool> IsExistsNotificationFriendAsync(Guid fromUserId, Guid toUserId);
}
