using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.MessageDTOs;
using MessengerServer.Src.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MessengerServer.Src.Infrastructure.Services;

public class HubMessageService(IHubContext<SignalRHub> signalRHub)
    : IHubMessageService
{
    private readonly IHubContext<SignalRHub> _signalRHub = signalRHub;

    public async Task SendMessageAsync(CreateMessageDTO message)
    {
        string method = message.Content != null ? "onMessage" : "onError";

        if (SignalRHub._connectionsMap.TryGetValue(message.SenderId, out string connectionUserInitId))
        {
            if (connectionUserInitId == null) return;
            await _signalRHub.Clients.Client(connectionUserInitId).SendAsync(method,
                message);
        }
        if (SignalRHub._connectionsMap.TryGetValue(message.ReceiverId, out string connectionUserReceiveId))
        {
            if (connectionUserReceiveId == null) return;
            await _signalRHub.Clients.Client(connectionUserReceiveId).SendAsync(method,
                message);
        }
        return;
    }
}
