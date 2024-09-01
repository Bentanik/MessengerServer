using MessengerServer.Src.Application.Common;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace MessengerServer.Src.Infrastructure.Repositories;

public class MessageRepository(AppDbContext context) :
    RepositoryBase<MessageEntity>(context),
    IMessageRepository
{
    private readonly AppDbContext _appDbContext = context;

    public async Task<Pagination<ViewMessageDTO>> GetMessageHistoryPagination
        (Guid senderId, Guid receiverId, int pageIndex = 0, int pageSize = 5)
    {
        var itemCount = await _appDbContext.Messages.CountAsync(m =>
              (m.SenderId == senderId && m.ReceiverId == receiverId) ||
              (m.SenderId == receiverId && m.ReceiverId == senderId));
        var messages = await _appDbContext.Messages.Where(m =>
              (m.SenderId == senderId && m.ReceiverId == receiverId) ||
              (m.SenderId == receiverId && m.ReceiverId == senderId)).OrderBy(x => x.CreatedDate)
                                .Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .AsNoTracking()
                                .Select(m => new ViewMessageDTO
                                {
                                    Content = m.Content,
                                    ReceiverId = m.ReceiverId,
                                    SenderId = m.SenderId,
                                })
                                .ToListAsync();

        var result = new Pagination<ViewMessageDTO>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = messages,
        };

        return result;
    }

    public async Task<Pagination<ViewMessageDTO>> GetLastMessageHistoryAsync
        (Guid senderId, Guid receiverId, int pageSize = 5)
    {
        var itemCount = await _appDbContext.Messages.CountAsync(m =>
             (m.SenderId == senderId && m.ReceiverId == receiverId) ||
             (m.SenderId == receiverId && m.ReceiverId == senderId));

        var lastPageIndex = (int)Math.Max(0, Math.Ceiling((double)itemCount / pageSize) - 1);

        var messages = await _appDbContext.Messages.Where(m =>
              (m.SenderId == senderId && m.ReceiverId == receiverId) ||
              (m.SenderId == receiverId && m.ReceiverId == senderId)).OrderBy(x => x.CreatedDate)
                                .Skip(lastPageIndex * pageSize)
                                .Take(pageSize)
                                .AsNoTracking()
                                .Select(m => new ViewMessageDTO
                                {
                                    Content = m.Content,
                                    ReceiverId = m.ReceiverId,
                                    SenderId = m.SenderId,
                                })
                                .ToListAsync();

        var result = new Pagination<ViewMessageDTO>()
        {
            PageIndex = lastPageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = messages,
        };
        return result;

    }

    public async Task<Pagination<ViewMessageDTO>> GetMessageHistory(Guid senderId, Guid receiverId)
    {
        var messages = await _appDbContext.Messages
        .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                    (m.SenderId == receiverId && m.ReceiverId == senderId))
        .OrderBy(x => x.CreatedDate)
        .AsNoTracking()
        .Select(m => new ViewMessageDTO
        {
            Content = m.Content,
            ReceiverId = m.ReceiverId,
            SenderId = m.SenderId,
        })
        .ToListAsync();

        var result = new Pagination<ViewMessageDTO>()
        {
            PageIndex = 1,
            PageSize = messages.Count,
            TotalItemsCount = messages.Count,
            Items = messages,
        };

        return result;
    }
}
