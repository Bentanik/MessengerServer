using MessengerServer.Src.Domain.Entities;
using MessengerServer.Src.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : RepositoryBase<User>(context), IUserRepository
{
    private readonly AppDbContext _appDbContext = context;

    public async Task<ViewEmailFullNameDTO> GetInfoEmailFullNameByEmail(string email)
    {
        return await _appDbContext.Users.Select(u => new ViewEmailFullNameDTO
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName
        }).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsUserExistsByEmail(string email)
    {
        return await _appDbContext.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsUserExistsByFullName(string fullName)
    {
        return await _appDbContext.Users.AnyAsync(u => u.FullName == fullName);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ViewUserProfilePrivateDTO> GetProfileUserPrivate(string email)
    {
        return await _appDbContext.Users.Select(u => new ViewUserProfilePrivateDTO
        {
           UserId = u.Id,
           Email = u.Email,
           FullName= u.FullName,
           Biography = "",
        }).FirstOrDefaultAsync(u => u.Email == email);
    }
}
