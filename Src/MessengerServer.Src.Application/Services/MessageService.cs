﻿using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.MapExtensions.MessageMapExtensions;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.Services;

public class MessageService(IHubMessageService hubMessageService, IUnitOfWork unitOfWork) : IMessageServices
{
    private readonly IHubMessageService _hubMessageService = hubMessageService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task UpdateChatHistory(Guid senderId, Guid receiverId, bool read)
    {
        var createChatHistoryDTO = new CreateChatHistoryDTO
        {
            UserId = senderId,
            ChatPartnerId = receiverId,
            Read = read,
        };
        var chatHistory = await _unitOfWork.ChatHistoryRepository.GetChatSenderAndRecieverHistory(createChatHistoryDTO);
        if (chatHistory == null)
        {
            var chatHistoryMapper = createChatHistoryDTO.ToChatHistory(); 
            _unitOfWork.ChatHistoryRepository.Add(chatHistoryMapper);
        }
        else
        {
            chatHistory.Read = true;
            _unitOfWork.ChatHistoryRepository.Update(chatHistory);
        }
        return;
    }

    public async Task SendMessageService(Guid userInitId, Guid userReceiveId, string content)
    {
        var message = new CreateMessageDTO
        {
            SenderId = userInitId,
            ReceiverId = userReceiveId,
            Content = content,
        };
        var messageMapper = message.ToMessageEntity();
        _unitOfWork.MessageRepository.Add(messageMapper);
        await UpdateChatHistory(userInitId, userReceiveId, true);
        await UpdateChatHistory(userReceiveId, userInitId, false);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0) message.Content = null;
        await _hubMessageService.SendMessageAsync(message);
        return;
    }
}
