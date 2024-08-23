using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using MessengerServer.Src.Contracts.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MessengerServer.Src.Application.Services;

public class ProfileUserServices(IUnitOfWork unitOfWork, IRedisService redisService, IEmailServices emailServices, IOptions<UpdateEmailSetting> updateEmailSetting, IOptions<ClientSetting> clientSetting, IOptions<MediaSetting> mediaSetting, IMediaService mediaService)
    : IProfileUserServices
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRedisService _redisService = redisService;
    private readonly IEmailServices _emailServices = emailServices;
    private readonly IMediaService _mediaService = mediaService;
    private readonly UpdateEmailSetting _updateEmailSetting = updateEmailSetting.Value;
    private readonly ClientSetting _clientSetting = clientSetting.Value;
    private readonly MediaSetting _mediaSetting = mediaSetting.Value;

    public async Task<Result<object>> GetProfilePrivateService(Guid userId)
    {
        var result = await _unitOfWork.UserRepository.GetProfileUserPrivateByUserIdAsync(userId);

        return new Result<object>
        {
            Error = result != null ? 0 : 1,
            Data = result,
        };
    }
    public async Task<Result<object>> GetProfilePublicService(Guid userId)
    {
        var result = await _unitOfWork.UserRepository.GetProfileUserPublicByUserIdAsync(userId);
        return new Result<object>
        {
            Error = result != null ? 0 : 1,
            Data = result
        };
    }
    public async Task<Result<object>> UpdateEmailUserService(Guid userId, string newEmail)
    {
        //Check email new Exist
        var isCheckEmailDuplicate = await _unitOfWork.UserRepository.IsUserExistsByEmailAsync(newEmail);
        if (isCheckEmailDuplicate)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.EmailExsits.GetMessage().Code,
                           ErrorMessage = MessagesList.EmailExsits.GetMessage().Message
                       }
                }
            };
        }

        var user = await _unitOfWork.UserRepository.GetProfileUserPrivateByUserIdAsync(userId);

        //Send mail
        var sendMail = await _emailServices.SendMailAsync(user.Email, "Update email", "UpdateEmail.html", new Dictionary<string, string> {
            { "ToEmail", user.Email},
            {"Link", $"{_clientSetting.ClientUrl}/{_clientSetting.ClientActiveUpdateEmail}/{user.Email}" }
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
                        ErrorCode = MessagesList.EmailSendingFailed.GetMessage().Code,
                        ErrorMessage = MessagesList.EmailSendingFailed.GetMessage().Message,
                    }
                }
            };
        }

        //Save redis
        await _redisService.SetStringAsync($"UpdateEmail:{user.Email}", newEmail, TimeSpan.FromMinutes(_updateEmailSetting.MinuteToUpdate));

        return new Result<object>
        {
            Error = 0,
            Message = $"Time update in {_updateEmailSetting.MinuteToUpdate} minute, please go to {user.Email} to change email",
        };
    }
    public async Task<Result<object>> ActiveUpdateEmailUserService(string email)
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
                       ErrorCode = MessagesList.UpdateEmailFail.GetMessage().Code,
                       ErrorMessage = MessagesList.UpdateEmailFail.GetMessage().Message
                   }
                }
            };
        }

        //Check email exists
        var isCheckEmail = await _unitOfWork.UserRepository.IsUserExistsByEmailAsync(newEmail);
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
                       ErrorCode = MessagesList.EmailExsits.GetMessage().Code,
                       ErrorMessage = MessagesList.EmailExsits.GetMessage().Message
                   }
                }
            };
        }

        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetMessage().Message
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
            Message = result > 0 ? MessagesList.UpdateEmailSuccess.GetMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UpdateEmailFail.GetMessage().Code,
                           ErrorMessage = MessagesList.UpdateEmailFail.GetMessage().Message
                       }
                    } : null
        };
    }
    public async Task<Result<object>> UpdateFullNameUserService(Guid userId, string newFullName)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetMessage().Message
                       }
                    }
            };
        }

        user.FullName = newFullName;
        var result = await _unitOfWork.SaveChangesAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.UpdateFullNameSuccess.GetMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
            {
                new()
                {
                    ErrorCode = MessagesList.UpdateFullNameFail.GetMessage().Code,
                    ErrorMessage = MessagesList.UpdateFullNameFail.GetMessage().Message
                }
            } : newFullName
        };
    }
    public async Task<Result<object>> UpdateBiographyUserService(Guid userId, string biography)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetMessage().Message
                       }
                    }
            };
        }
        user.Biography = biography;
        var result = await _unitOfWork.SaveChangesAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.UpdateBiographySuccess.GetMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UpdateBiographyFail.GetMessage().Code,
                           ErrorMessage = MessagesList.UpdateBiographyFail.GetMessage().Message
                       }
                    } : null
        };
    }
    public async Task<Result<object>> UpdateAvatarUserService(Guid userId, string nameFile, IFormFile cropAvatarFile, IFormFile fullAvatarFile)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetMessage().Message
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
                           ErrorCode = MessagesList.UploadAvatarFail.GetMessage().Code,
                           ErrorMessage = MessagesList.UploadAvatarFail.GetMessage().Message
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
            Message = result > 0 ? MessagesList.UploadAvatarSuccessfully.GetMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
            {
                new()
                {
                    ErrorCode = MessagesList.UploadAvatarFail.GetMessage().Code,
                    ErrorMessage = MessagesList.UploadAvatarFail.GetMessage().Message
                }
            } : $"{pathSave}/{newCropAvatarName}"
        };
    }
    public async Task<Result<object>> UpdateCoverPhotoService(Guid userId, string nameFile, IFormFile cropCoverPhotoFile, IFormFile fullCoverPhotoFile)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.UserNotExist.GetMessage().Code,
                           ErrorMessage = MessagesList.UserNotExist.GetMessage().Message
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
                           ErrorCode = MessagesList.UploadAvatarFail.GetMessage().Code,
                           ErrorMessage = MessagesList.UploadAvatarFail.GetMessage().Message
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
            Message = result > 0 ? MessagesList.UploadAvatarSuccessfully.GetMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
            {
                new()
                {
                    ErrorCode = MessagesList.UploadAvatarFail.GetMessage().Code,
                    ErrorMessage = MessagesList.UploadAvatarFail.GetMessage().Message
                }
            } : $"{pathSave}/{newCropCoverPhotoName}"
        };
    }
}
