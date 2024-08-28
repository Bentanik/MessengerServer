using MessengerServer.Src.Contracts.Abstractions;

namespace MessengerServer.Src.Application.Interfaces;

public interface IUserServices
{
    Task<Result<object>> AddFriendService(Guid userId1, Guid userId2);
    Task<Result<object>> AcceptFriendService(Guid userInitId, Guid userReceiveId);
    Task<Result<object>> RejectFriendService(Guid userInitId, Guid userReceiveId);
    Task<Result<object>> GetStatusFriendService(Guid userId1, Guid userId2);
    Task<Result<object>> UnfriendService(Guid userInitId, Guid userReceiveId);
}
