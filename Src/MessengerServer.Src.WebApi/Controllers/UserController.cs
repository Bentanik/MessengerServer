using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserServices userServices, INotificationService notificationService)
    : ControllerBase
{
    private readonly IUserServices _userServices = userServices;
    private readonly INotificationService _notificationService = notificationService;

    [Authorize]
    [HttpPost("add_friend")]
    public async Task<IActionResult> AddFriendApi([FromQuery] Guid friendUserId)
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
        var result = await _userServices.AddFriendService(Guid.Parse(userId), friendUserId);
        if (result.Error == 1)
        {
            return BadRequest(result);
        }
        ViewUserAddedFriendDTO viewUserAddedFriendDTO = result.Data as ViewUserAddedFriendDTO;
        var addNotificationFriendDTO = new AddNotificationFriendDTO
        {
            FromUserId = Guid.Parse(userId),
            ToUserId = friendUserId,
        };
        await _notificationService.PushNotificationFriend(viewUserAddedFriendDTO, addNotificationFriendDTO);
        result.Data = null;
        return Ok(result);
    }

    [Authorize]
    [HttpGet("get_status_friend")]
    public async Task<IActionResult> GetStatusFriendApi([FromQuery] Guid friendUserId)
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
        var result = await _userServices.GetStatusFriendService(Guid.Parse(userId), friendUserId);
        if (result.Error == 1)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}
