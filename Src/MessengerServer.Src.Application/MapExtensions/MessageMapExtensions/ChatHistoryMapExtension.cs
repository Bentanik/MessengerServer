using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.MapExtensions.MessageMapExtensions;

public static class ChatHistoryMapExtension
{
    public static ChatHistoryEntity ToChatHistory(this CreateChatHistoryDTO createChatDto)
    {
        return new ChatHistoryEntity
        {
            UserId = createChatDto.UserId,
            ChatPartnerId = createChatDto.ChatPartnerId,
            Read = createChatDto.Read,
        };
    }
}
