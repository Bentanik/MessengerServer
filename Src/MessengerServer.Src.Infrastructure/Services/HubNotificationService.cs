using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MessengerServer.Src.Infrastructure.Services;

public class HubNotificationService(IHubContext<NotificationHub> notificationHub) 
    : IHubNotificationService
{
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;

    public async Task CountNotificationAsync(Guid userId, int countNotification)
    {
        if (NotificationHub._connectionsMap.TryGetValue(userId, out string connectionId))
        {
            if (connectionId == null) return;
            await _notificationHub.Clients.Client(connectionId).SendAsync("onCountNotification", 
                countNotification);
        }
        return;
    }

    public async Task SendNotificationFriendAsync(Guid userId, ViewUserAddedFriendDTO viewUserAddedFriendDTO)
    {
        if (NotificationHub._connectionsMap.TryGetValue(userId, out string connectionId))
        {
            if (connectionId == null) return;
            await _notificationHub.Clients.Client(connectionId).SendAsync("onFriendNotification", viewUserAddedFriendDTO);
        }
        return;
    }
}
