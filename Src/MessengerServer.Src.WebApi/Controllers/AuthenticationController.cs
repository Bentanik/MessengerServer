using MessengerServer.Src.Application;
using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using Microsoft.AspNetCore.Mvc;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationServices _authenticationService;
    public AuthenticationController(IAuthenticationServices authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var registerDto = req.ToRegisterDTO();
        var result = await _authenticationService.Register(registerDto);
        return Ok(result);
    }
}
