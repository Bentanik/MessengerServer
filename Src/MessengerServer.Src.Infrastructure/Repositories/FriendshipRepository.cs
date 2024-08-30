using MessengerServer.Src.Domain.Entities;
using MessengerServer.Src.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using MessengerServer.Src.Contracts.DTOs.FriendshipDTOs;
using MessengerServer.Src.Domain.Enum;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Infrastructure.Repositories;

public class FriendshipRepository(AppDbContext context) :
    RepositoryBase<FriendshipEntity>(context),
    IFriendshipRepository
{
    private readonly AppDbContext _appDbContext = context;

    public async Task<bool> IsExistFriend(Guid userInitId, Guid userReceiveId)
    {
        return await _appDbContext.Friendships.AnyAsync(fr => fr.UserInitId == userInitId && fr.UserReceiveId == userReceiveId ||
                   fr.UserInitId == userReceiveId && fr.UserReceiveId == userInitId);
    }

    public async Task<GetFriendshipDTO> GetFriendshipDTOAsync(Guid userInitId, Guid userReceiveId)
    {
        return await _appDbContext.Friendships.AsNoTracking()
            .Where(fr => fr.UserInitId == userInitId && fr.UserReceiveId == userReceiveId ||
                    fr.UserInitId == userReceiveId && fr.UserReceiveId == userInitId)
            .Select(fr => new GetFriendshipDTO
            {
                Id = fr.Id,
                UserInitId = fr.UserInitId,
                UserReceiveId = fr.UserReceiveId,
                IsUserInit = fr.UserInitId == userInitId,
                Status = fr.Status,
            })
            .FirstOrDefaultAsync();
    }

    public async Task<FriendshipEntity> GetFriendshipAsync(Guid userInitId, Guid userReceiveId)
    {
        return await _appDbContext.Friendships
            .AsNoTracking()
            .FirstOrDefaultAsync(fr => fr.UserInitId == userInitId && fr.UserReceiveId == userReceiveId ||
                 fr.UserInitId == userReceiveId && fr.UserReceiveId == userInitId
            );
    }

    public async Task<int> GetNumbersOfFriendByUserIdAsync(Guid userId)
    {
        return await _appDbContext.Friendships
            .Where(fr => (fr.UserInitId == userId || fr.UserReceiveId == userId) && fr.Status == FriendshipStatusEnum.Accepted)
            .CountAsync();
    }
    public async Task<List<ViewUserProfileDTO>> GetNineFriendsByUserIdAsync(Guid userId)
    {
        return await _appDbContext.Friendships
            .OrderByDescending(fr => fr.CreatedDate)
            .Take(9)
            .Where(fr => (fr.UserInitId == userId || fr.UserReceiveId == userId) && fr.Status == FriendshipStatusEnum.Accepted)
            .Select(fr => new ViewUserProfileDTO
            {
                UserId = userId != fr.UserInitId ? userId : fr.UserInitId,
                FullName = userId != fr.UserInitId ? fr.UserInitiated.FullName : fr.UserReceived.FullName,
                CropAvatar = userId != fr.UserInitId ? fr.UserInitiated.CropAvatar : fr.UserReceived.CropAvatar,
            }).ToListAsync();
    }
}
