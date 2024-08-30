using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Application.Repositories;

public interface IMediaService
{
    Task<bool> SaveAvatarCoverPhotoAsync(string pathSave, string cropMediaName, string fullMediaName ,IFormFile cropMediaFile, IFormFile fullMediaFile);
    IEnumerable<string> GetTopNFiles(int numbers, string path);
}
