using MessengerServer.Src.Application.Common;
using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface IMessageRepository : IRepositoryBase<MessageEntity>
{
    Task<Pagination<ViewMessageDTO>> GetMessageHistory
        (Guid senderId, Guid receiverId);
    Task<Pagination<ViewMessageDTO>> GetMessageHistoryPagination
           (Guid senderId, Guid receiverId, int pageIndex = 0, int pageSize = 10);
    Task<Pagination<ViewMessageDTO>> GetLastMessageHistoryAsync(Guid senderId, Guid receiverId, int pageSize = 5);
}
