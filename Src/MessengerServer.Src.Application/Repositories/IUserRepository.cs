using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface IUserRepository : IRepositoryBase<UserEntity>
{
    Task<bool> IsUserExistsByEmailAsync(string email);
    Task<bool> IsUserExistsByFullNameAsync(string fullName);
    Task<ViewEmailFullNameDTO> GetInfoEmailFullNameByEmailAsync(string email);
    Task<UserEntity> GetUserByEmailAsync(string email);
    Task<ViewUserProfilePrivateDTO> GetProfileUserPrivateByUserIdAsync(Guid userId);
    Task<ViewUserProfileDTO> GetProfileUserPublicByUserIdAsync(Guid UserId);
    Task<ViewUserAddedFriendDTO> GetInfoUserAddedFriendByUserIdAsync(Guid userId);
}
