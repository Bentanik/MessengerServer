
namespace MessengerServer.Src.Application.Interfaces;

public interface IMessageServices
{
    Task SendMessageService(Guid userInitId, Guid userReceiveId, string content);
}
