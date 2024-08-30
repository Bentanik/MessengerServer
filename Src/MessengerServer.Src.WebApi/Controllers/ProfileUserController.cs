using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.Abstractions.EditUserRequests;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileUserController(IProfileUserServices profileUserServices) : ControllerBase
{
    private readonly IProfileUserServices _profileUserServices = profileUserServices;

    [Authorize]
    [HttpGet("get_profile_private")]
    public async Task<IActionResult> GetProfileUserPrivateApi()
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
        var result = await _profileUserServices.GetProfilePrivateService(Guid.Parse(userId));
        return Ok(result);
    }

    [HttpGet("get_profile_public")]
    public async Task<IActionResult> GetProfileUserPublicApi([FromQuery] Guid userId)
    {
        var result = await _profileUserServices.GetProfilePublicService(userId);
        return Ok(result);
    }

    [HttpGet("get_numbers_friend")]
    public async Task<IActionResult> GetNumbersOfFriendApi([FromQuery] Guid userId)
    {
        var result = await _profileUserServices.GetNumbersOfFriendService(userId);
        return Ok(result);
    }

    [HttpGet("get_nine_friends")]
    public async Task<IActionResult> GetNineFriendsApi([FromQuery] Guid userId)
    {
        var result = await _profileUserServices.GetNineFriendsService(userId);
        return Ok(result);
    }

    [HttpGet("get_nine_images")]
    public async Task<IActionResult> GetNineImagesApi([FromQuery] Guid userId)
    {
        var result = await _profileUserServices.GetNineImagesService(userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_email")]
    public async Task<IActionResult> UpdateEmailUserApi([FromBody] EditEmailRequest req)
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

        var result = await _profileUserServices.UpdateEmailUserService(Guid.Parse(userId), req.Email);

        if (result.Error == 1)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("active_update_email")]
    public async Task<IActionResult> ActiveUpdateEmailUserApi([FromQuery] string email)
    {
        var result = await _profileUserServices.ActiveUpdateEmailUserService(email);
        if (result.Error == 1)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_fullname")]
    public async Task<IActionResult> UpdateFullNameUserApi([FromBody] EditFullNameRequest req)
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

        var result = await _profileUserServices.UpdateFullNameUserService(Guid.Parse(userId), req.FullName);

        if (result.Error == 1)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_biography")]
    public async Task<IActionResult> UpdateBiographyUserApi([FromBody] EditBiographyRequest req)
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

        var result = await _profileUserServices.UpdateBiographyUserService(Guid.Parse(userId), req.Biography);

        if (result.Error == 1)
        {
            return BadRequest(result);
        };

        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_avatar")]
    public async Task<IActionResult> UpdateAvatarUserApi([FromForm] EditAvatarRequest req)
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

        var result = await _profileUserServices.UpdateAvatarUserService(Guid.Parse(userId), req.CropFileAvatar, req.FullFileAvatar);

        if (result.Error == 1)
        {
            return BadRequest(result);
        };

        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_cover_photo")]
    public async Task<IActionResult> UpdateCoverPhotoApi([FromForm] EditCoverPhotoRequest req)
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

        var result = await _profileUserServices.UpdateCoverPhotoService(Guid.Parse(userId), req.CropFileCoverPhoto, req.FullFileCoverPhoto);

        if (result.Error == 1)
        {
            return BadRequest(result);
        };

        return Ok(result);
    }
}
