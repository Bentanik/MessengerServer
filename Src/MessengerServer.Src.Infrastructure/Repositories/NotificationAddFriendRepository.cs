using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerServer.Src.Infrastructure.Repositories;
public class NotificationAddFriendRepository(AppDbContext context) : 
    RepositoryBase<NotificationAddFriendEntitiy>(context), 
    INotificationAddFriendRepository
{
    private readonly AppDbContext _appDbContext = context;

    public async Task<int> CountNotificationAddedFriendByStatusAsync(Guid userId)
    {
        return await _appDbContext.NotificationAddFriends
            .CountAsync(nt => nt.ToUserId == userId && nt.Status == false);
    }
}
