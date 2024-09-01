using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions.MessageRequests;
using Microsoft.AspNetCore.SignalR;

namespace MessengerServer.Src.WebApi.Hubs;

public class SignalRHub(INotificationService notificationService, IMessageServices messageServices) 
    : Hub
{
    public readonly static Dictionary<Guid, string> _connectionsMap = [];
    private readonly INotificationService _notificationService = notificationService;
    private readonly IMessageServices _messageServices = messageServices;

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

    public async Task SendMessageAsync(SendMessageRequest req)
    {
        await _messageServices.SendMessageService(req.UserInitId, req.UserReceiveId, req.Content);
    }

    public async Task GetMessageHistoryAsync(GetMessageRequest req)
    {
        var result = await _messageServices.GetMessageHistoryService(req.UserInitId, req.UserReceiveId);
        await Clients.Caller.SendAsync("onMessageHistory", result);
    }
}
