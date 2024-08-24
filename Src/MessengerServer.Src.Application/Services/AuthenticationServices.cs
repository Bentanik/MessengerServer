using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;
using MessengerServer.Src.Contracts.MessagesList;
using MessengerServer.Src.Contracts.ErrorResponses;
using System.Text.Json;
using MessengerServer.Src.Application.MapExtensions.AuthenticationMapExtensions;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.Contracts.Settings;
using Microsoft.Extensions.Options;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationResponses;

namespace MessengerServer.Src.Application.Services;

public class AuthenticationServices(IPasswordHashService passwordHash, IUnitOfWork unitOfWork, IEmailServices emailServices, IRedisService redisService, ITokenGeneratorService tokenGeneratorService, IOptions<ClientSetting> clientSetting, IOptions<JwtSetting> jwtSetting, IOptions<MediaSetting> mediaSetting) 
    : IAuthenticationServices
{
    private readonly IPasswordHashService _passwordHash = passwordHash;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailServices _emailServices = emailServices;
    private readonly IRedisService _redisService = redisService;
    private readonly ClientSetting _clientSetting = clientSetting.Value;
    private readonly JwtSetting _jwtSetting = jwtSetting.Value;
    private readonly ITokenGeneratorService _tokenGeneratorService = tokenGeneratorService;
    private readonly MediaSetting _mediaSetting = mediaSetting.Value;

    public async Task<Result<object>> RegisterService(RegisterDTO registerDto)
    {
        var emailExsits = await _unitOfWork.UserRepository.IsUserExistsByEmailAsync(registerDto.Email);
        var fullNameExsits = await _unitOfWork.UserRepository.IsUserExistsByFullNameAsync(registerDto.FullName);

        //Check duplicate email or full name
        if (emailExsits || fullNameExsits)
        {
            var listError = new List<ErrorResponse>();
            //If email duplicate
            if (emailExsits)
            {
                listError.Add(new()
                {
                    ErrorCode = MessagesList.EmailExsits.GetMessage().Code,
                    ErrorMessage = MessagesList.EmailExsits.GetMessage().Message
                });
            }
            //If full name duplicate
            if (fullNameExsits)
            {
                listError.Add(new()
                {
                    ErrorCode = MessagesList.FullNameExsits.GetMessage().Code,
                    ErrorMessage = MessagesList.FullNameExsits.GetMessage().Message,
                });
            };

            return new Result<object>
            {
                Error = 1,
                Data = listError
            };
        }
        //Hash password
        registerDto.Password = _passwordHash.HashPassword(registerDto.Password);

        //Send mail
        var sendMail = await _emailServices.SendMailAsync(registerDto.Email, "Sign up account", "EmailRegister.html", new Dictionary<string, string> {
            { "ToEmail", registerDto.Email},
            {"Link", $"{_clientSetting.ClientUrl}/{_clientSetting.ClientActiveAccount}/{registerDto.Email}" }
        });

        //Email not send
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

        //Save information into redis
        await _redisService.SetStringAsync($"rg_{registerDto.Email}", JsonSerializer.Serialize(registerDto), TimeSpan.FromHours(12));

        return new Result<object>
        {
            Error = 0,
            Message = MessagesList.Register.GetMessage().Message,
            Data = null
        };
    }
    public async Task<Result<object>> ActiveAccountService(string email)
    {
        var emailExsits = await _unitOfWork.UserRepository.IsUserExistsByEmailAsync(email);
        //Email exists in the system 
        if (emailExsits)
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

        //Get object in redis
        var redisResult = _redisService.GetStringAsync($"rg_{email}").Result;
        //Check object empty
        if (redisResult == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                {
                   new()
                   {
                       ErrorCode = MessagesList.ActiveRegisterFail.GetMessage().Code,
                       ErrorMessage = MessagesList.ActiveRegisterFail.GetMessage().Message
                   }
                }
            };
        }

        var userDto = JsonSerializer.Deserialize<RegisterDTO>(redisResult);

        if (userDto != null) userDto.UserId = Guid.NewGuid();

        var userMapper = userDto?.ToUser();

        if (userMapper != null)
        {
            userMapper.CropAvatar = $"{_mediaSetting.PathSystem}/crop_avatar_unknown.jpg";
            userMapper.FullAvatar = $"{_mediaSetting.PathSystem}/full_avatar_unknown.jpg";
            _unitOfWork.UserRepository.Add(userMapper);
        }

        var result = await _unitOfWork.SaveChangesAsync();
        if (result != 0)
        {
            await _redisService.DeleteKeyAsync($"rg_{email}");
        }

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.RegisterSuccess.GetMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.RegisterFail.GetMessage().Code,
                           ErrorMessage = MessagesList.RegisterFail.GetMessage().Message
                       }
                    } : null
        };
    }
    public async Task<Result<object>> LoginService(LoginDTO loginDto)
    {
        var emailExsits = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginDto.Email);
        //Check email not exist
        if (emailExsits == null)
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

        //Verify password
        var verifyPassword = _passwordHash.VeriyPassword(loginDto.Password, emailExsits.Password);
        if (verifyPassword == false)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.PasswordNotMatch.GetMessage().Code,
                           ErrorMessage = MessagesList.PasswordNotMatch.GetMessage().Message
                       }
                    }
            };
        }

        UserGenerateTokenDTO userTokenGenerate = new()
        {
            UserId = emailExsits.Id,
            RoleName = "UserEntity",
        };

        //Generate access token
        var accessToken = _tokenGeneratorService.GenerateAccessToken(userTokenGenerate);
        //Generate refresh token
        var refreshToken = _tokenGeneratorService.GenerateRefreshToken(userTokenGenerate);

        await _redisService.SetStringAsync($"RefreshTokenService:{emailExsits.Id}", refreshToken, TimeSpan.FromMinutes(_jwtSetting.RefreshTokenExpMinute));

        var loginTokenDTO = new LoginTokenDTO()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        var viewHeaderUserDTO = new ViewHeaderUserDTO()
        {
            UserId = emailExsits.Id,
            FullName = emailExsits.FullName,
            Email = emailExsits.Email,
            Avatar = emailExsits.CropAvatar,
        };

        var loginResponse = new LoginResponse()
        {
            ViewHeaderUserDTO = viewHeaderUserDTO,
            LoginTokenDTO = loginTokenDTO
        };

        return new Result<object>
        {
            Error = 0,
            Data = loginResponse
        };
    }
    public async Task<Result<object>> LogoutService(string userId)
    {
        await _redisService.DeleteKeyAsync($"RefreshTokenService:{userId}");
        return new Result<object>
        {
            Error = 0,
            Message = "LogoutService successfully",
        };
    }
    public async Task<Result<object>> RefreshTokenService(string token)
    {
        var userId = _tokenGeneratorService.ValidateAndGetUserIdFromRefreshToken(token);
        if (userId == null)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.LoginTimeout.GetMessage().Code,
                           ErrorMessage = MessagesList.LoginTimeout.GetMessage().Message
                       }
                    }
            };
        }

        var oldRefreshToken = await _redisService.GetStringAsync($"RefreshTokenService:{userId}");
        if (token != oldRefreshToken)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.LoginTimeout.GetMessage().Code,
                           ErrorMessage = MessagesList.LoginTimeout.GetMessage().Message
                       }
                    }
            };
        }

        UserGenerateTokenDTO userTokenGenerate = new()
        {
            UserId = Guid.Parse(userId),
            RoleName = "UserEntity",
        };

        var accessToken = _tokenGeneratorService.GenerateAccessToken(userTokenGenerate);
        var refreshToken = _tokenGeneratorService.GenerateRefreshToken(userTokenGenerate);
        await _redisService.SetStringAsync($"RefreshTokenService:{userId}", refreshToken, TimeSpan.FromMinutes(_jwtSetting.RefreshTokenExpMinute));

        var loginTokenDTO = new LoginTokenDTO()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        var loginResponse = new LoginResponse()
        {
            LoginTokenDTO = loginTokenDTO
        };

        return new Result<object>
        {
            Error = 0,
            Message = "Refresh token successfully",
            Data = loginResponse
        };
    }
}
