using MessengerServer.Src.Application.Repositories;
using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Infrastructure.Services;

public class MediaService : IMediaService
{
    public async Task<bool> SaveAvatarAsync(string pathSave, string cropAvatarName, string fullAvatarName, IFormFile cropAvatarFile, IFormFile fullAvatarFile)
    {
        if ((cropAvatarFile == null || cropAvatarFile.Length == 0) && (fullAvatarFile == null || fullAvatarFile.Length == 0))
            return false;
        try
        {
            string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), pathSave);

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            //Save crop avatar
            if (cropAvatarFile != null && cropAvatarFile.Length > 0)
            {
                string croppedFilePath = Path.Combine(uploadsFolderPath, cropAvatarName);
                using var fileStream = new FileStream(croppedFilePath, FileMode.Create);
                await cropAvatarFile.CopyToAsync(fileStream);
            }

            //Save full avatar
            if (fullAvatarFile != null && fullAvatarFile.Length > 0)
            {
                string fullFilePath = Path.Combine(uploadsFolderPath, fullAvatarName);
                using var fileStream = new FileStream(fullFilePath, FileMode.Create);
                await fullAvatarFile.CopyToAsync(fileStream);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
