using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;
using MessengerServer.Src.Contracts.MessagesList;
using MessengerServer.Src.Contracts.ErrorResponses;
using System.Text.Json;
using MessengerServer.Src.Application.MapExtensions.AuthenticationMapExtensions;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using MessengerServer.Src.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace MessengerServer.Src.Application.Services;

public class AuthenticationServices(IPasswordHash passwordHash, IUnitOfWork unitOfWork, IEmailServices emailServices, IRedisService redisService, ITokenGeneratorService tokenGeneratorService, IOptions<ClientSetting> clientSetting) : IAuthenticationServices
{
    private readonly IPasswordHash _passwordHash = passwordHash;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailServices _emailServices = emailServices;
    private readonly IRedisService _redisService = redisService;
    private readonly ClientSetting _clientSetting = clientSetting.Value;
    private readonly ITokenGeneratorService _tokenGeneratorService = tokenGeneratorService;

    public async Task<Result<object>> Register(RegisterDTO registerDto)
    {
        var emailExsits = await _unitOfWork.UserRepository.IsUserExistsByEmail(registerDto.Email);
        var fullNameExsits = await _unitOfWork.UserRepository.IsUserExistsByFullName(registerDto.FullName);

        //Check duplicate email or full name
        if (emailExsits || fullNameExsits)
        {
            var listError = new List<ErrorResponse>();
            //If email duplicate
            if (emailExsits)
            {
                listError.Add(new()
                {
                    ErrorCode = MessagesList.EmailExsits.GetErrorMessage().Code,
                    ErrorMessage = MessagesList.EmailExsits.GetErrorMessage().Message
                });
            }
            //If full name duplicate
            if (fullNameExsits)
            {
                listError.Add(new()
                {
                    ErrorCode = MessagesList.FullNameExsits.GetErrorMessage().Code,
                    ErrorMessage = MessagesList.FullNameExsits.GetErrorMessage().Message,
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
        var sendMail = await _emailServices.SendMail(registerDto.Email, "Sign up account", "EmailRegister.html", new Dictionary<string, string> {
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
                        ErrorCode = MessagesList.EmailSendingFailed.GetErrorMessage().Code,
                        ErrorMessage = MessagesList.EmailSendingFailed.GetErrorMessage().Message,
                    }
                }
            };
        }

        //Save information into redis
        await _redisService.SetStringAsync($"rg_{registerDto.Email}", JsonSerializer.Serialize(registerDto), TimeSpan.FromHours(12));

        return new Result<object>
        {
            Error = 0,
            Message = MessagesList.Register.GetErrorMessage().Message,
            Data = null
        };
    }

    public async Task<Result<object>> ActiveAccount(string email)
    {
        var emailExsits = await _unitOfWork.UserRepository.IsUserExistsByEmail(email);
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
                       ErrorCode = MessagesList.EmailExsits.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.EmailExsits.GetErrorMessage().Message
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
                       ErrorCode = MessagesList.ActiveRegisterFail.GetErrorMessage().Code,
                       ErrorMessage = MessagesList.ActiveRegisterFail.GetErrorMessage().Message
                   }
                }
            };
        }

        var userDto = JsonSerializer.Deserialize<RegisterDTO>(redisResult);

        if (userDto != null) userDto.UserId = Guid.NewGuid();

        var userMapper = userDto?.ToUser();
        if (userMapper != null) _unitOfWork.UserRepository.Add(userMapper);

        var result = await _unitOfWork.SaveChangesAsync();
        if (result != 0)
        {
            await _redisService.DeleteKeyAsync($"rg_{email}");
        }

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.RegisterSuccess.GetErrorMessage().Message : null,
            Data = result == 0 ? new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.RegisterFail.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.RegisterFail.GetErrorMessage().Message
                       }
                    } : null
        };
    }

    public async Task<Result<object>> Login(LoginDTO loginDto)
    {
        var emailExsits = await _unitOfWork.UserRepository.GetUserByEmail(loginDto.Email);
        //Check email exist
        if (emailExsits == null)
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
        
        //Verify password
        var verifyPassword = _passwordHash.VeriyPassword(loginDto.Password, emailExsits.Password);
        if(verifyPassword == false)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.PasswordNotMatch.GetErrorMessage().Code,
                           ErrorMessage = MessagesList.PasswordNotMatch.GetErrorMessage().Message
                       }
                    }
            };
        }

        UserGenerateTokenDTO userTokenGenerate = new()
        {
            Email = loginDto.Email,
            RoleName = "User",
        };

        //Generate access token
        var accessToken = _tokenGeneratorService.GenerateAccessToken(userTokenGenerate);
        //Generate refresh token
        var refreshToken = _tokenGeneratorService.GenerateRefreshToken(userTokenGenerate);

        var loginResponse = new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
        if (emailExsits?.Id != null) loginResponse.UserId = emailExsits.Id;
        return new Result<object>
        {
            Error = 0,
            Data = loginResponse
        };
    }
}
