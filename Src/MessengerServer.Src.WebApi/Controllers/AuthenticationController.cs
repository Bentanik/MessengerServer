using Azure;
using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.MapExtensions.AuthenticationMapExtensions;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationResponses;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController(IAuthenticationServices authenticationService) : ControllerBase
{
    private readonly IAuthenticationServices _authenticationService = authenticationService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var registerDto = req.ToRegisterDTO();
        var result = await _authenticationService.Register(registerDto);
        return Ok(result);
    }

    [HttpGet("active_account")]
    public async Task<IActionResult> ActiveAccount([FromQuery] string email)
    {
        var result = await _authenticationService.ActiveAccount(email);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var loginDTO = req.ToLoginDTO();
        var serviceLogin = await _authenticationService.Login(loginDTO);

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
                           ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
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
            Message = "Login successfully",
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

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var result = await _authenticationService.Logout(email);
        Response.Cookies.Delete("refreshToken");
        return Ok(result);
    }

    [HttpPut("refresh_token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
            return Unauthorized(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.LoginTimeout.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.LoginTimeout.GetErrorMessage().Message
                       }
                    }
            });

        var serviceRefreshToken = await _authenticationService.RefreshToken(refreshToken);
        if (serviceRefreshToken.Error == 1)
            return Unauthorized(serviceRefreshToken);

        LoginResponse? response = serviceRefreshToken?.Data as LoginResponse;

        if (response == null)
        {
            return Ok(new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.LoginAgain.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.LoginAgain.GetErrorMessage().Message
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
