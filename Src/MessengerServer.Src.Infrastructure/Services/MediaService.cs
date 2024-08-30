using MessengerServer.Src.Application.Repositories;
using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Infrastructure.Services;

public class MediaService : IMediaService
{
    public async Task<bool> SaveAvatarCoverPhotoAsync(string pathSave, string cropMediaName, string fullMediaName, IFormFile cropMediaFile, IFormFile fullMediaFile)
    {
        if ((cropMediaFile == null || cropMediaFile.Length == 0) && (fullMediaFile == null || fullMediaFile.Length == 0))
            return false;
        try
        {
            string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), pathSave);

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            //Save crop Media
            if (cropMediaFile != null && cropMediaFile.Length > 0)
            {
                string croppedFilePath = Path.Combine(uploadsFolderPath, cropMediaName);
                using var fileStream = new FileStream(croppedFilePath, FileMode.Create);
                await cropMediaFile.CopyToAsync(fileStream);
            }

            //Save full Media
            if (fullMediaFile != null && fullMediaFile.Length > 0)
            {
                string fullFilePath = Path.Combine(uploadsFolderPath, fullMediaName);
                using var fileStream = new FileStream(fullFilePath, FileMode.Create);
                await fullMediaFile.CopyToAsync(fileStream);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<string> GetTopNFiles(int numbers, string path)
    {
        if (!Directory.Exists(path))
        {
            return null;
        }
        var files = Directory.GetFiles(path);
        var sortedFiles = files
           .Where(file => !Path.GetFileNameWithoutExtension(file).Contains("crop"))
           .Select(file => new
           {
               FilePath = $"http://localhost:5200/api/image/get_image_avatar?fileName={file}",
               UnixTimestamp = long.TryParse(Path.GetFileNameWithoutExtension(file).Split('_').FirstOrDefault(), out var timestamp) ? timestamp : 0
           })
           .OrderByDescending(x => x.UnixTimestamp)
           .Take(numbers)
           .Select(x => x.FilePath)
           .ToList();

        return sortedFiles;
    }
}
