using MessengerServer.Src.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MessengerServer.Src.Application.Interfaces;

public interface IEditUserServices
{
    Task<Result<object>> GetProfilePrivate(string email);
    Task<Result<object>> UpdateEmailUser(string oldEmail, string newEmail);
    Task<Result<object>> ActiveUpdateEmailUser(string email);
    Task<Result<object>> UpdateFullNameUser(string email, string newFullName);
    Task<Result<object>> UpdateBiographyUser(string email, string biography);

    Task<Result<object>> UpdateAvatarUser(string email, string nameFile, IFormFile cropAvatarFile, IFormFile fullAvatarFile);
    Task<Result<object>> UpdateCoverPhoto(string email, string nameFile, IFormFile cropCoverPhotoFile, IFormFile fullCoverPhotoFile);

}
