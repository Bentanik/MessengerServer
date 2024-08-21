using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using MessengerServer.Src.Contracts.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MessengerServer.Src.Application.Services;

public class EditUserServices(IUnitOfWork unitOfWork, IRedisService redisService, IEmailServices emailServices, IOptions<UpdateEmailSetting> updateEmailSetting, IOptions<ClientSetting> clientSetting, IOptions<MediaSetting> mediaSetting, IMediaService mediaService)
    : IEditUserServices
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRedisService _redisService = redisService;
    private readonly IEmailServices _emailServices = emailServices;
    private readonly IMediaService _mediaService = mediaService;
    private readonly UpdateEmailSetting _updateEmailSetting = updateEmailSetting.Value;
    private readonly ClientSetting _clientSetting = clientSetting.Value;
    private readonly MediaSetting _mediaSetting = mediaSetting.Value;

    public async Task<Result<object>> GetProfilePrivate(string email)
    {
        var result = await _unitOfWork.UserRepository.GetProfileUserPrivate(email);

        return new Result<object>
        {
            Error = result != null ? 0 : 1,
            Data = result,
        };
    }

    public async Task<Result<object>> UpdateEmailUser(string oldEmail, string newEmail)
    {
        //Check email new Exist
        var isCheckEmailDuplicate = await _unitOfWork.UserRepository.IsUserExistsByEmail(newEmail);

        if (isCheckEmailDuplicate)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.EmailExsits.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.EmailExsits.GetErrorMessage().Message
                       }
                }
            };
        }

        //Send mail
        var sendMail = await _emailServices.SendMail(oldEmail, "Update email", "UpdateEmail.html", new Dictionary<string, string> {
            { "ToEmail", oldEmail},
            {"Link", $"{_clientSetting.ClientUrl}/{_clientSetting.ClientActiveUpdateEmail}/{oldEmail}" }
        });

        if (!sendMail)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Send email fail",
                Data = new List<ErrorResponse>
                {
                    new() {
                        ErrorCode = MessagesList.EmailSendingFailed.GetErrorMessage().Code,
                        ErrorMessage = MessagesList.EmailSendingFailed.GetErrorMessage().Message,
                    }
                }
            };
        }

        //Save redis
        await _redisService.SetStringAsync($"UpdateEmail:{oldEmail}", newEmail, TimeSpan.FromMinutes(_updateEmailSetting.MinuteToUpdate));

        return new Result<object>
        {
            Error = 0,
            Message = $"Time update in {_updateEmailSetting.MinuteToUpdate} minute, please go to {oldEmail} to change email",
        };
    }

    public async Task<Result<object>> ActiveUpdateEmailUser(string email)
    {
        var newEmail = await _redisService.GetStringAsync($"UpdateEmail:{email}");
        if (newEmail == null)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Update fail!",
                Data = new List<ErrorResponse>
                {
                   new()
                   {
                       ErrorCode = MessagesList.UpdateEmailFail.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.UpdateEmailFail.GetErrorMessage().Message
                   }
                }
            };
        }

        //Check email exists
        var isCheckEmail = await _unitOfWork.UserRepository.IsUserExistsByEmail(newEmail);
        if (isCheckEmail)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
                Data = new List<ErrorResponse>
                {
                   new()
                   {
                       ErrorCode = MessagesList.EmailExsits.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.EmailExsits.GetErrorMessage().Message
                   }
                }
            };
        }

        var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetErrorMessage().Message
                       }
                    }
            };
        }
        user.Email = newEmail;
        var result = await _unitOfWork.SaveChangesAsync();
        if (result > 0)
            await _redisService.DeleteKeyAsync($"UpdateEmail:{email}");

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.UpdateEmailSuccess.GetErrorMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UpdateEmailFail.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UpdateEmailFail.GetErrorMessage().Message
                       }
                    } : null
        };
    }

    public async Task<Result<object>> UpdateFullNameUser(string email, string newFullName)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetErrorMessage().Message
                       }
                    }
            };
        }
        user.FullName = newFullName;
        var result = await _unitOfWork.SaveChangesAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.UpdateFullNameSuccess.GetErrorMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UpdateFullNameFail.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UpdateFullNameFail.GetErrorMessage().Message
                       }
                    } : null
        };
    }

    public async Task<Result<object>> UpdateBiographyUser(string email, string biography)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetErrorMessage().Message
                       }
                    }
            };
        }
        user.Biography = biography;
        var result = await _unitOfWork.SaveChangesAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.UpdateBiographySuccess.GetErrorMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UpdateBiographyFail.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UpdateBiographyFail.GetErrorMessage().Message
                       }
                    } : null
        };
    }

    public async Task<Result<object>> UpdateAvatarUser(string email, string nameFile, IFormFile cropAvatarFile, IFormFile fullAvatarFile)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetErrorMessage().Message
                       }
                    }
            };
        }

        DateTime dateTime = DateTime.UtcNow;
        long unixTimestamp = new DateTimeOffset(dateTime).ToUnixTimeSeconds();

        string fileExtension = Path.GetExtension(cropAvatarFile.FileName);
        string newCropAvatarName = $"crop_{nameFile}_{unixTimestamp}{fileExtension}";
        string newFullAvatarName = $"full_{nameFile}_{unixTimestamp}{fileExtension}";
        string pathSave = $"{_mediaSetting.PathUser}/{user.Id}";

        var isSaveAvatar = await _mediaService.SaveAvatarCoverPhotoAsync(pathSave, newCropAvatarName, newFullAvatarName, cropAvatarFile, fullAvatarFile);
        if (isSaveAvatar == false)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UploadAvatarFail.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UploadAvatarFail.GetErrorMessage().Message
                       }
                    }
            };
        }
      

        user.CropAvatar =  $"{pathSave}/{newCropAvatarName}";
        user.FullAvatar = $"{pathSave}/{newFullAvatarName}";

        _unitOfWork.UserRepository.Update(user);
        var result = await _unitOfWork.SaveChangesAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.UploadAvatarSuccessfully.GetErrorMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
            {
                new()
                {
                    ErrorCode = MessagesList.UploadAvatarFail.GetErrorMessage().Code,
                    ErrorMessage = MessagesList.UploadAvatarFail.GetErrorMessage().Message
                }
            } : $"{pathSave}/{newCropAvatarName}"
        };
    }

    public async Task<Result<object>> UpdateCoverPhoto(string email, string nameFile, IFormFile cropCoverPhotoFile, IFormFile fullCoverPhotoFile)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetErrorMessage().Message
                       }
                    }
            };
        }

        DateTime dateTime = DateTime.UtcNow;
        long unixTimestamp = new DateTimeOffset(dateTime).ToUnixTimeSeconds();

        string fileExtension = Path.GetExtension(cropCoverPhotoFile.FileName);
        string newCropCoverPhotoName = $"crop_{nameFile}_{unixTimestamp}{fileExtension}";
        string newFullCoverPhotoName = $"full_{nameFile}_{unixTimestamp}{fileExtension}";
        string pathSave = $"{_mediaSetting.PathUser}/{user.Id}";

        var isSaveAvatar = await _mediaService.SaveAvatarCoverPhotoAsync(pathSave, newCropCoverPhotoName, newFullCoverPhotoName, cropCoverPhotoFile, fullCoverPhotoFile);
        if (isSaveAvatar == false)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UploadAvatarFail.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.UploadAvatarFail.GetErrorMessage().Message
                       }
                    }
            };
        }


        user.CropCoverPhoto = $"{pathSave}/{newCropCoverPhotoName}";
        user.FullCoverPhoto = $"{pathSave}/{newFullCoverPhotoName}";

        _unitOfWork.UserRepository.Update(user);
        var result = await _unitOfWork.SaveChangesAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.UploadAvatarSuccessfully.GetErrorMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
            {
                new()
                {
                    ErrorCode = MessagesList.UploadAvatarFail.GetErrorMessage().Code,
                    ErrorMessage = MessagesList.UploadAvatarFail.GetErrorMessage().Message
                }
            } : $"{pathSave}/{newCropCoverPhotoName}"
        };
    }
}
