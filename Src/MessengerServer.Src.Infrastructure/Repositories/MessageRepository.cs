using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Infrastructure.Repositories;

public class MessageRepository(AppDbContext context) :
    RepositoryBase<MessageEntity>(context),
    IMessageRepository
{
    private readonly AppDbContext _appDbContext = context;
}
