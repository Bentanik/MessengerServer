using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/notification")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [Authorize]
    [HttpGet("get_two_notification_friend")]
    public async Task<IActionResult> GetTwoNotificationFriend()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (userId == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>()
                {
                   new()
                   {
                       ErrorCode = MessagesList.LoginAgain.GetMessage().Code,
                       ErrorMessage = MessagesList.LoginAgain.GetMessage().Message
                   }
                }
            });
        }
        var result = await _notificationService.GetTwoNotificationFriendService(Guid.Parse(userId));
        return Ok(result);
    }
}
