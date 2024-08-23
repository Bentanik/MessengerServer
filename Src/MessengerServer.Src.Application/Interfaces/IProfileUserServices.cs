using MessengerServer.Src.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Application.Interfaces;

public interface IProfileUserServices
{
    Task<Result<object>> GetProfilePrivateService(Guid userId);
    Task<Result<object>> GetProfilePublicService(Guid userId);
    Task<Result<object>> UpdateEmailUserService(Guid userId, string newEmail);
    Task<Result<object>> ActiveUpdateEmailUserService(string email);
    Task<Result<object>> UpdateFullNameUserService(Guid userId, string newFullName);
    Task<Result<object>> UpdateBiographyUserService(Guid userId, string biography);
    Task<Result<object>> UpdateAvatarUserService(Guid userId, string nameFile, IFormFile cropAvatarFile, IFormFile fullAvatarFile);
    Task<Result<object>> UpdateCoverPhotoService(Guid userId, string nameFile, IFormFile cropCoverPhotoFile, IFormFile fullCoverPhotoFile);
}
