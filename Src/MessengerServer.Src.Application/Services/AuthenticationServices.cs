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

namespace MessengerServer.Src.Application.Services;

public class AuthenticationServices(IPasswordHash passwordHash, IUnitOfWork unitOfWork, IEmailServices emailServices, IRedisService redisService, ITokenGeneratorService tokenGeneratorService) : IAuthenticationServices
{
    private readonly IPasswordHash _passwordHash = passwordHash;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailServices _emailServices = emailServices;
    private readonly IRedisService _redisService = redisService;
    private readonly ITokenGeneratorService _tokenGeneratorService = tokenGeneratorService;

    public async Task<Result<object>> Register(RegisterDTO registerDto)
    {
        var emailExsits = await _unitOfWork.UserRepository.IsUserExistsByEmail(registerDto.Email);
        //Email exists in the system 
        if (emailExsits)
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

        var fullNameExsits = await _unitOfWork.UserRepository.IsUserExistsByFullName(registerDto.FullName);
        //FullName exists in the system 
        if (fullNameExsits)
        {
            return new Result<object>
            {
                Error = 1,
                Data = new List<ErrorResponse>
                {
                    new() {
                        ErrorCode = MessagesList.FullNameExsits.GetErrorMessage().Code,
                        ErrorMessage = MessagesList.FullNameExsits.GetErrorMessage().Message,
                    }
                }
            };
        }

        //Hash password
        registerDto.Password = _passwordHash.HashPassword(registerDto.Password);

        //Send mail
        var sendMail = await _emailServices.SendMail(registerDto.Email, "Sign up account", "EmailRegister.html", new Dictionary<string, string> {
            { "ToEmail", registerDto.Email},
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

        var redisResult = _redisService.GetStringAsync($"rg_{email}").Result;
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

        if(userDto != null) userDto.UserId = Guid.NewGuid();

        var userMapper = userDto?.ToUser();
        if (userMapper != null) _unitOfWork.UserRepository.Add(userMapper);

        var result = await _unitOfWork.SaveChangesAsync();
        if(result != 0)
        {
            await _redisService.DeleteKeyAsync($"rg_{email}");
        }

        UserGenerateTokenDTO userTokenGenerate = new()
        {
            Email = email,
            RoleName = "User",
        };
        
        //Generate access token
        var accessToken =  _tokenGeneratorService.GenerateAccessToken(userTokenGenerate);
        //Generate refresh token
        var refreshToken = _tokenGeneratorService.GenerateRefreshToken(userTokenGenerate);

        var loginResponse = new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
        if(userDto?.UserId != null) loginResponse.UserId = userDto.UserId;

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.RegisterSuccess.GetErrorMessage().Message : MessagesList.RegisterFail.GetErrorMessage().Message,
            Data = loginResponse
        };
    }

    public async Task<Result<object>> Login(LoginDTO loginDto)
    {
        var emailExsits = await _unitOfWork.UserRepository.GetUserByEmail(loginDto.Email);

        if(emailExsits == null)
        {
            return new Result<object>
            {
                Error = 1,
                Message = MessagesList.UserNotExist.GetErrorMessage().Message,
            };
        }

        UserGenerateTokenDTO userTokenGenerate = new()
        {
            Email = loginDto.Email,
            RoleName = "User",
        };


        var result = 0;
        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? MessagesList.RegisterSuccess.GetErrorMessage().Message : MessagesList.RegisterFail.GetErrorMessage().Message,
            //Data = loginResponse
        };
    }
}
