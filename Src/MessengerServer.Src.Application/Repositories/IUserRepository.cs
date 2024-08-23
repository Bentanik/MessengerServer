using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> IsUserExistsByEmailAsync(string email);
    Task<bool> IsUserExistsByFullNameAsync(string fullName);
    Task<ViewEmailFullNameDTO> GetInfoEmailFullNameByEmailAsync(string email);
    Task<User> GetUserByEmailAsync(string email);
    Task<ViewUserProfilePrivateDTO> GetProfileUserPrivateByUserIdAsync(Guid userId);
    Task<ViewUserProfileDTO> GetProfileUserPublicByUserIdAsync(Guid UserId);

}
