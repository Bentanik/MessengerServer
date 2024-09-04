using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Repositories;

public interface IChatHistoryRepository : IRepositoryBase<ChatHistoryEntity>
{
    Task<ChatHistoryEntity> GetChatSenderAndRecieverHistory(CreateChatHistoryDTO createChatHistoryDto);
    Task<IEnumerable<ViewTheseFriendsMessagesDTO>> GetTheseFriendMessagesByUserId(Guid userId);
}
