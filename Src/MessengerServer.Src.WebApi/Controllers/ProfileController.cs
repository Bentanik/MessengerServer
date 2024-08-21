using MessengerServer.Src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IProfileServices _profileServices;

    public ProfileController(IProfileServices profileServices)
    {
        _profileServices = profileServices;
    }

    [HttpGet("get_profile_user")]
    public async Task<IActionResult> GetProfileUser([FromQuery]string email)
    {
        var result = await _profileServices.GetProfileUser(email);
        return Ok(result);
    }
}
