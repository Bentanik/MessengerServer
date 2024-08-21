using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.Abstractions.EditUserRequests;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/user/edit_user")]
public class EditUserController(IEditUserServices editUserServices) : ControllerBase
{
    private readonly IEditUserServices _editUserServices = editUserServices;

    [Authorize]
    [HttpGet("get_profile_private")]
    public async Task<IActionResult> GetProfilePrivate()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if(email == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>()
                {
                   new()
                   {
                       ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
                   }
                }
            });
        }
        var result = await _editUserServices.GetProfilePrivate(email);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_email")]
    public async Task<IActionResult> UpdateEmailUser([FromBody] EditEmailRequest req)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>()
                {
                   new()
                   {
                       ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
                   }
                }
            });
        }

        var result = await _editUserServices.UpdateEmailUser(email, req.Email);

        if(result.Error == 1)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet("active_update_email")]
    public async Task<IActionResult> ActiveUpdateEmailUser([FromQuery] string email)
    {
        var result = await _editUserServices.ActiveUpdateEmailUser(email);
        if(result.Error == 1)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_fullname")]
    public async Task<IActionResult> UpdateFullNameUser([FromBody] EditFullNameRequest req)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>()
                {
                   new()
                   {
                       ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
                   }
                }
            });
        }

        var result = await _editUserServices.UpdateFullNameUser(email, req.FullName);

        if (result.Error == 1)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_biography")]
    public async Task<IActionResult> UpdateBiographyUser([FromBody] EditBiographyRequest req)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>()
                {
                   new()
                   {
                       ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
                   }
                }
            });
        }

        var result = await _editUserServices.UpdateBiographyUser(email, req.Biography);

        if (result.Error == 1)
        {
            return BadRequest(result);
        };

        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_avatar")]
    public async Task<IActionResult> UpdateAvatarUser([FromForm] EditAvatarRequest req)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>()
                {
                   new()
                   {
                       ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
                   }
                }
            });
        }

        var result = await _editUserServices.UpdateAvatarUser(email, req.FileName, req.CropFileAvatar, req.FullFileAvatar);

        if (result.Error == 1)
        {
            return BadRequest(result);
        };

        return Ok(result);
    }

    [Authorize]
    [HttpPost("update_cover_photo")]
    public async Task<IActionResult> UpdateCoverPhoto([FromForm] EditCoverPhotoRequest req)
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>()
                {
                   new()
                   {
                       ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
                   }
                }
            });
        }

        var result = await _editUserServices.UpdateCoverPhoto(email, req.FileName, req.CropFileCoverPhoto, req.FullFileCoverPhoto);

        if (result.Error == 1)
        {
            return BadRequest(result);
        };

        return Ok(result);
    }
}
