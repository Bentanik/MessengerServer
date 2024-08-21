using MessengerServer.Src.Contracts.Abstractions;

namespace MessengerServer.Src.Application.Interfaces;

public interface IProfileServices
{
    Task<Result<object>> GetProfileUser(string email);
}
