using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface INotificationAddFriendRepository : IRepositoryBase<NotificationAddFriendEntitiy>
{
    Task<int> CountNotificationAddedFriendByStatusAsync(Guid userId);
}
