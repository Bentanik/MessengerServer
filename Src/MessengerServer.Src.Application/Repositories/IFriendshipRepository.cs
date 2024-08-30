using MessengerServer.Src.Contracts.DTOs.FriendshipDTOs;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface IFriendshipRepository : IRepositoryBase<FriendshipEntity>
{
    Task<bool> IsExistFriend(Guid userInitId, Guid userReceiveId);
    Task<FriendshipEntity> GetFriendshipAsync(Guid userInitId, Guid userReceiveId);
    Task<GetFriendshipDTO> GetFriendshipDTOAsync(Guid userInitId, Guid userReceiveId);
    Task<int> GetNumbersOfFriendByUserIdAsync(Guid userId);
    Task<List<ViewUserProfileDTO>> GetNineFriendsByUserIdAsync(Guid userId);
}
