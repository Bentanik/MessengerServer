using MessengerServer.Src.Application;
using MessengerServer.Src.Application.Repositories;

namespace MessengerServer.Src.Infrastructure;

public class UnitOfWork(AppDbContext appDbContext, IUserRepository userRepository, IFriendshipRepository friendshipRepository, INotificationAddFriendRepository notificationAddFriendRepository, IMessageRepository messageRepository, IChatHistoryRepository chatHistoryRepository)
    : IUnitOfWork
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFriendshipRepository _friendshipRepository = friendshipRepository;
    private readonly INotificationAddFriendRepository _notificationAddFriendRepository = notificationAddFriendRepository;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IChatHistoryRepository _chatHistoryRepository = chatHistoryRepository;

    public IUserRepository UserRepository => _userRepository;
    public IFriendshipRepository FriendshipRepository => _friendshipRepository;
    public INotificationAddFriendRepository NotificationAddFriendRepository => _notificationAddFriendRepository;
    public IMessageRepository MessageRepository => _messageRepository;
    public IChatHistoryRepository ChatHistoryRepository => _chatHistoryRepository;

    public async Task<int> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }
}
