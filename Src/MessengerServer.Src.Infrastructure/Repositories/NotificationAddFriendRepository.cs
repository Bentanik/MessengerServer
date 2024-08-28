using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
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

    public async Task<NotificationAddFriendEntitiy> GetNotificationAddFriendAsync(Guid fromUserId, Guid toUserId)
    {
        return await _appDbContext.NotificationAddFriends
            .AsNoTracking()
            .FirstOrDefaultAsync(nt => nt.FromUserId == fromUserId && nt.ToUserId == toUserId ||
                nt.FromUserId == toUserId && nt.ToUserId == fromUserId
            );
    }

    public async Task<IEnumerable<ViewNotificationAddFriendDTO>> GetTwoInfoUserNotificationFriendAsync(Guid userId)
    {
        using var transaction = await _appDbContext.Database.BeginTransactionAsync();

        try
        {
            var notifications = await _appDbContext.NotificationAddFriends
                .AsNoTracking()
                .Include(nt => nt.FromUser)
                .Where(nt => nt.ToUserId == userId) 
                .OrderByDescending(nt => nt.CreatedDate)
                .Take(2)
                .ToListAsync();

            var notificationIds = notifications.Select(nt => nt.Id).ToList();

            if (notificationIds.Any())
            {
                await _appDbContext.NotificationAddFriends
                    .Where(nt => notificationIds.Contains(nt.Id) && !nt.Status)
                    .ForEachAsync(nt => nt.Status = true);

                await _appDbContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return notifications.Select(nt => new ViewNotificationAddFriendDTO
            {
                NotficiationAddFriendId = nt.Id,
                UserId = nt.FromUserId,
                CropAvatar = nt.FromUser.CropAvatar,
                FullName = nt.FromUser.FullName,
            });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> IsExistsNotificationFriendAsync(Guid fromUserId, Guid toUserId)
    {
        return await _appDbContext.NotificationAddFriends
           .AnyAsync(nt => nt.FromUserId == fromUserId && nt.ToUserId == toUserId ||
               nt.FromUserId == toUserId && nt.ToUserId == fromUserId
           );
    }
}
