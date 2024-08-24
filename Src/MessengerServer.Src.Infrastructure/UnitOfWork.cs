using MessengerServer.Src.Application;
using MessengerServer.Src.Application.Repositories;

namespace MessengerServer.Src.Infrastructure;

public class UnitOfWork(AppDbContext appDbContext, IUserRepository userRepository, IFriendshipRepository friendshipRepository) 
    : IUnitOfWork
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFriendshipRepository _friendshipRepository = friendshipRepository;

    public IUserRepository UserRepository => _userRepository;
    public IFriendshipRepository FriendshipRepository => _friendshipRepository;

    public async Task<int> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }
}
