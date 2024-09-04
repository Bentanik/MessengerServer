
using MessengerServer.Src.Contracts.Abstractions;

namespace MessengerServer.Src.Application.Interfaces;

public interface IMessageServices
{
    Task SendMessageService(Guid userInitId, Guid userReceiveId, string content);
    Task<Result<object>> GetMessageHistoryService(Guid userInitId, Guid userReceiveId);
    Task<Result<object>> GetLastMessageHistoryService(Guid userInitId, Guid userReceiveId);
    Task<Result<object>> GetTheseFriendMessagesService(Guid userId);
}
