using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerServer.Src.Infrastructure.Repositories;

public class ChatHistoryRepository(AppDbContext context) :
    RepositoryBase<ChatHistoryEntity>(context),
    IChatHistoryRepository
{
    private readonly AppDbContext _appDbContext = context;

    public async Task<ChatHistoryEntity> GetChatSenderAndRecieverHistory(CreateChatHistoryDTO createChatHistoryDto)
    {
        return await _appDbContext.ChatHistories.FirstOrDefaultAsync
            (ch => ch.UserId == createChatHistoryDto.UserId && ch.ChatPartnerId == createChatHistoryDto.ChatPartnerId);
    }
}
