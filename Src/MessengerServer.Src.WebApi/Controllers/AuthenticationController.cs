using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.MapExtensions.AuthenticationMapExtensions;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
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

        if(result?.Error == 1)
        {
            return Ok(result);
        }

        LoginResponse? response = null;

        if(result?.Data != null)
        {
            response = result?.Data as LoginResponse;
        }
        
        if (response == null)
        {
            return Ok(new Result<object>
            {
                Error = 1,
                Message = "Please login again!",
                Data = null
            });
        }

        if(response.RefreshToken != null)
        {
            Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.Strict,
            });
        }

        return Ok(new Result<object>
        {
            Error = 0,
            Message = "Login successfully",
            Data = new
            {
                AccessToken = new
                {
                    Token_type = "Bearer",
                    Token = response.AccessToken,
                },
            }
        });
    }
}
