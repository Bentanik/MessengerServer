using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/message")]
public class MessageController(IMessageServices messageService) : ControllerBase
{
    private readonly IMessageServices _messageService = messageService;

    [Authorize]
    [HttpGet("get_message_history")]
    public async Task<IActionResult> GetMessageHistory([FromQuery] Guid userReceiveId)
    {
        var userId = User.FindFirstValue("UserId");
        if (userId == null)
        {
            Response.Cookies.Delete("refreshToken");
            return Ok(new Result<object>
            {
                Error = 0,
            });
        }
        var result = await _messageService.GetMessageHistoryService(Guid.Parse(userId), userReceiveId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("get_last_message_history")]
    public async Task<IActionResult> GetLastMessageHistory([FromQuery] Guid userReceiveId)
    {
        var userId = User.FindFirstValue("UserId");
        if (userId == null)
        {
            Response.Cookies.Delete("refreshToken");
            return Ok(new Result<object>
            {
                Error = 0,
            });
        }
        var result = await _messageService.GetLastMessageHistoryService(Guid.Parse(userId), userReceiveId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("get_these_friends_messaged")]
    public async Task<IActionResult> GetTheseFriendMessaged([FromQuery] Guid userReceiveId)
    {
        var userId = User.FindFirstValue("UserId");
        if (userId == null)
        {
            Response.Cookies.Delete("refreshToken");
            return Ok(new Result<object>
            {
                Error = 0,
            });
        }
        var result = await _messageService.GetTheseFriendMessagesService(Guid.Parse(userId));
        return Ok(result);
    }
}
