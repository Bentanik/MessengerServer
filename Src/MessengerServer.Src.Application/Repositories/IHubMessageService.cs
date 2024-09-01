using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.MessageDTOs;

namespace MessengerServer.Src.Application.Repositories;

public interface IHubMessageService
{
    Task SendMessageAsync(CreateMessageDTO message);
}
