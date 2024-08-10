using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.MapExtensions.AuthenticationMapExtensions;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using Microsoft.AspNetCore.Mvc;

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

        if(serviceLogin?.Error == 1)
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

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
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
                Token_type = "Bearer",
                AccessToken = response.AccessToken
            }
        });
    }
}
