using MessengerServer.Src.Domain.Entities;
using MessengerServer.Src.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : RepositoryBase<UserEntity>(context), IUserRepository
{
    private readonly AppDbContext _appDbContext = context;
    public async Task<ViewEmailFullNameDTO> GetInfoEmailFullNameByEmailAsync(string email)
    {
        return await _appDbContext.Users.Where(u => u.Email == email).Select(u => new ViewEmailFullNameDTO
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName
        }).FirstOrDefaultAsync();
    }
    public async Task<bool> IsUserExistsByEmailAsync(string email)
    {
        return await _appDbContext.Users.AnyAsync(u => u.Email == email);
    }
    public async Task<bool> IsUserExistsByFullNameAsync(string fullName)
    {
        return await _appDbContext.Users.AnyAsync(u => u.FullName == fullName);
    }
    public async Task<UserEntity> GetUserByEmailAsync(string email)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<ViewUserProfilePrivateDTO> GetProfileUserPrivateByUserIdAsync(Guid userId)
    {
        return await _appDbContext.Users.AsNoTracking().Where(u => u.Id == userId).Select(u => new ViewUserProfilePrivateDTO
        {
            UserId = u.Id,
            Email = u.Email,
            FullName = u.FullName,
            Biography = u.Biography ?? "",
        }).FirstOrDefaultAsync();
    }
    public async Task<ViewUserProfileDTO> GetProfileUserPublicByUserIdAsync(Guid UserId)
    {
        return await _appDbContext.Users.AsNoTracking().Where(u => u.Id == UserId).Select(u => new ViewUserProfileDTO
        {
            UserId = u.Id,
            FullName = u.FullName,
            CropCoverPhoto = u.CropCoverPhoto,
            CropAvatar = u.CropAvatar,
        }).FirstOrDefaultAsync();
    }
    public async Task<ViewUserAddedFriendDTO> GetInfoUserAddedFriendByUserIdAsync(Guid userId)
    {
        return await _appDbContext.Users.Where(u => u.Id == userId).Select(u => new ViewUserAddedFriendDTO
        {
            UserId = u.Id,
            FullName = u.FullName,
            CropAvatar = u.CropAvatar,
        }).FirstOrDefaultAsync();
    }
}
