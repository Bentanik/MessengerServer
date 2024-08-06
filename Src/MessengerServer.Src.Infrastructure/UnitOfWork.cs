using MessengerServer.Src.Application;
using MessengerServer.Src.Application.Repositories;

namespace MessengerServer.Src.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private readonly IUserRepository _userRepository;
    public UnitOfWork(AppDbContext appDbContext, IUserRepository userRepository)
    {
        _appDbContext = appDbContext;
        _userRepository = userRepository;
    }

    public IUserRepository UserRepository => _userRepository;

    public async Task<int> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }
}
