using MessengerServer.Src.Application.Repositories;

namespace MessengerServer.Src.Application;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    IUserRepository UserRepository { get; }
    IFriendshipRepository FriendshipRepository { get; }
    INotificationAddFriendRepository NotificationAddFriendRepository { get; }
    IMessageRepository MessageRepository { get; }
    IChatHistoryRepository ChatHistoryRepository { get; }
}
