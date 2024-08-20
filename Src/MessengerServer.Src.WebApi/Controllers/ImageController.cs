using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : ControllerBase
{
    [HttpGet("get_image_avatar")]
    public IActionResult GetImageAvatar([FromQuery] string fileName)
    {
        if (fileName.IsNullOrEmpty() == true)
        {
            return NotFound(new
            {
                err = 1,
                mess = "Please upload image again!"
            });
        }

        var filePath = Path.Combine(fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return File(fileStream, "image/jpeg");
    }
}
