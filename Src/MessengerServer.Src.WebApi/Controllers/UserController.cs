using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetHome()
    {
        return Ok("Ok");
    }
}
