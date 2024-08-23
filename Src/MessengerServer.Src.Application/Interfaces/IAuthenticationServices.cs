using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

namespace MessengerServer.Src.Application.Interfaces;

public interface IAuthenticationServices
{
    Task<Result<object>> RegisterService(RegisterDTO registerDto);
    Task<Result<object>> ActiveAccountService(string email);
    Task<Result<object>> LoginService(LoginDTO loginDto);
    Task<Result<object>> LogoutService(string userId);
    Task<Result<object>> RefreshTokenService(string token);
}
