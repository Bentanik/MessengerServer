using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> IsUserExistsByEmail(string email);
    Task<bool> IsUserExistsByFullName(string fullName);
    Task<ViewEmailFullNameDTO> GetInfoEmailFullNameByEmail(string email);
    Task<User> GetUserByEmail(string email);
}
