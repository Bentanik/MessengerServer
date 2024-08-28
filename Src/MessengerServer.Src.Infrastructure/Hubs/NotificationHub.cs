using MessengerServer.Src.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MessengerServer.Src.WebApi.Hubs;

public class NotificationHub(INotificationService notificationService) : Hub
{
    public readonly static Dictionary<Guid, string> _connectionsMap = [];
    private readonly INotificationService _notificationService = notificationService;

    public override async Task OnConnectedAsync()
    {
        try
        {
            var userId = Guid.Parse(Context.GetHttpContext().Request.Query["userId"]);
            if (userId != null)
            {
                _connectionsMap[userId] = Context.ConnectionId;
                await Clients.Caller.SendAsync("onSuccess", "Successfully connected.");
            }
            else
            {
                await Clients.Caller.SendAsync("onError", "User ID not found.");
            }
            Console.WriteLine(userId);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("onError", $"message: {ex.Message}");
        }

        await base.OnConnectedAsync();
    }
    public async Task CountNotification(Guid userId)
    {
        await _notificationService.CountNotificationService(userId);
    }
}
