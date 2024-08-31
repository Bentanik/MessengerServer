using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MessengerServer.Src.Infrastructure.Services;

public class HubNotificationService(IHubContext<SignalRHub> signalRHub) 
    : IHubNotificationService
{
    private readonly IHubContext<SignalRHub> _signalRHub = signalRHub;

    public async Task CountNotificationAsync(Guid userId, int countNotification)
    {
        if (SignalRHub._connectionsMap.TryGetValue(userId, out string connectionId))
        {
            if (connectionId == null) return;
            await _signalRHub.Clients.Client(connectionId).SendAsync("onCountNotification", 
                countNotification);
        }
        return;
    }

    public async Task SendNotificationFriendAsync(Guid userId, ViewUserAddedFriendDTO viewUserAddedFriendDTO)
    {
        if (SignalRHub._connectionsMap.TryGetValue(userId, out string connectionId))
        {
            if (connectionId == null) return;
            await _signalRHub.Clients.Client(connectionId).SendAsync("onFriendNotification", viewUserAddedFriendDTO);
        }
        return;
    }
}
