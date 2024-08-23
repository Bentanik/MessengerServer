using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.MapExtensions.AuthenticationMapExtensions;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationResponses;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController(IAuthenticationServices authenticationService) : ControllerBase
{
    private readonly IAuthenticationServices _authenticationService = authenticationService;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterApi([FromBody] RegisterRequest req)
    {
        var registerDto = req.ToRegisterDTO();
        var result = await _authenticationService.RegisterService(registerDto);
        return Ok(result);
    }

    [HttpGet("active_account")]
    public async Task<IActionResult> ActiveAccountApi([FromQuery] string email)
    {
        var result = await _authenticationService.ActiveAccountService(email);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginApi([FromBody] LoginRequest req)
    {
        var loginDTO = req.ToLoginDTO();
        var serviceLogin = await _authenticationService.LoginService(loginDTO);

        if (serviceLogin?.Error == 1)
        {
            return Ok(serviceLogin);
        }

        LoginResponse? response = serviceLogin?.Data as LoginResponse;

        if (response == null)
        {
            return Ok(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.LoginAgain.GetMessage().Code,
                           ErrorMessage = MessagesList.LoginAgain.GetMessage().Message
                       }
                    }
            });
        }

        Response.Cookies.Append("refreshToken", response.LoginTokenDTO.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Path = "/",
            SameSite = SameSiteMode.Strict,
        });

        return Ok(new Result<object>
        {
            Error = 0,
            Message = "LoginService successfully",
            Data = new
            {
                Token = new
                {
                    TokenType = "Bearer",
                    response.LoginTokenDTO.AccessToken,
                },
                User = response.ViewHeaderUserDTO,
            }
        });
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> LogoutApi()
    {
        var userId = User.FindFirstValue("UserId");
        if(userId == null)
        {
            Response.Cookies.Delete("refreshToken");
            return Ok(new Result<object>
            {
                Error = 0,
            });
        }
        var result = await _authenticationService.LogoutService(userId);
        Response.Cookies.Delete("refreshToken");
        return Ok(result);
    }

    [HttpPut("refresh_token")]
    public async Task<IActionResult> RefreshTokenApi()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            return Unauthorized(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.LoginTimeout.GetMessage().Code,
                           ErrorMessage = MessagesList.LoginTimeout.GetMessage().Message
                       }
                    }
            });
        }
            

        var serviceRefreshToken = await _authenticationService.RefreshTokenService(refreshToken);
        if (serviceRefreshToken.Error == 1)
            return Unauthorized(serviceRefreshToken);

        LoginResponse? response = serviceRefreshToken?.Data as LoginResponse;

        if (response == null)
        {
            return Unauthorized(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.LoginAgain.GetMessage().Code,
                           ErrorMessage = MessagesList.LoginAgain.GetMessage().Message
                       }
                    }
            });
        }

        Response.Cookies.Append("refreshToken", response.LoginTokenDTO.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Path = "/",
            SameSite = SameSiteMode.Strict,
        });

        return Ok(new Result<object>
        {
            Error = 0,
            Message = "Refresh token successfully",
            Data = new
            {
                Token = new
                {
                    TokenType = "Bearer",
                    response.LoginTokenDTO.AccessToken,
                },
                User = response.ViewHeaderUserDTO,
            }
        });
    }
}
