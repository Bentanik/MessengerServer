using MessengerServer.Src.Contracts.Abstractions;

namespace MessengerServer.Src.Application.Interfaces;

public interface IUserServices
{
    Task<Result<object>> AddFriendService(Guid userId1, Guid userId2);
    Task<Result<object>> GetStatusFriendService(Guid userId1, Guid userId2);
}
