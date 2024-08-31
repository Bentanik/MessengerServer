using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.MapExtensions.MessageMapExtensions;

public static class MessageMapExtension
{
    public static MessageEntity ToMessageEntity(this CreateMessageDTO createMessageDto)
    {
        return new MessageEntity
        {
            SenderId = createMessageDto.SenderId,
            ReceiverId = createMessageDto.ReceiverId,
            Content = createMessageDto.Content,
        };
    }
}
