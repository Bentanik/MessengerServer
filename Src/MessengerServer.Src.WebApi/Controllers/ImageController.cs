using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MessengerServer.Src.WebApi.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : ControllerBase
{
    private readonly string _imagePath;
    public ImageController()
    {
        _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Avatar");
        if (!Directory.Exists(_imagePath))
        {
            Directory.CreateDirectory(_imagePath);
        }
    }

    [HttpGet("get_image")]
    public async Task<IActionResult> GetImage([FromQuery] string fileName)
    {
        if (fileName.IsNullOrEmpty() == true)
        {
            return NotFound(new
            {
                err = 1,
                mess = "Please upload image again!"
            });
        }

        var filePath = Path.Combine(_imagePath, fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return File(fileStream, "image/jpeg");
    }
}
