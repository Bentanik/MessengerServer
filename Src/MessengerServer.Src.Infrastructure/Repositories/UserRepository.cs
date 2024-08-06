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
}
