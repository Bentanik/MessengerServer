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

    public async Task<IEnumerable<ViewTheseFriendsMessagesDTO>> GetTheseFriendMessagesByUserId(Guid userId)
    {
        var result = await _appDbContext.ChatHistories
         .Where(ch => ch.UserId == userId)
         .Select(ch => new
         {
             PartnerId = ch.ChatPartnerId,
             PartnerCropAvatar = ch.ChatPartner.CropAvatar,
             FullName = ch.ChatPartner.FullName,
             LastMessage = _appDbContext.Messages
                 .Where(m => (m.SenderId == userId && m.ReceiverId == ch.ChatPartnerId) ||
                             (m.SenderId == ch.ChatPartnerId && m.ReceiverId == userId))
                 .OrderByDescending(m => m.CreatedDate)
                 .FirstOrDefault()
         })
         .GroupBy(x => x.PartnerId)
         .Select(g => new ViewTheseFriendsMessagesDTO
         {
             PartnerId = g.Select(x => x.PartnerId).FirstOrDefault(),
             CropAvatar = "http://localhost:5200/api/image/get_image_avatar?fileName=" + g.Select(x => x.PartnerCropAvatar).FirstOrDefault(),
             FullName = g.Select(x => x.FullName).FirstOrDefault(),
             Content = g.Select(x => x.LastMessage).FirstOrDefault().Content,
         })
         .ToListAsync();

        return result;
    }
}
