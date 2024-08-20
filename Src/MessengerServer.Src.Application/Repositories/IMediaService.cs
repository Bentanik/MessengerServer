using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Application.Repositories;

public interface IMediaService
{
    Task<bool> SaveAvatarAsync(string pathSave, string cropAvatarName, string fullAvatarName ,IFormFile cropAvatarFile, IFormFile FullAvatarFile);
}
