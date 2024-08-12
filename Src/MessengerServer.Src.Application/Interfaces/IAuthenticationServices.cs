using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

namespace MessengerServer.Src.Application.Interfaces;

public interface IAuthenticationServices
{
    Task<Result<object>> Register(RegisterDTO registerDto);
    Task<Result<object>> ActiveAccount(string email);
    Task<Result<object>> Login(LoginDTO loginDto);
    Task<Result<object>> Logout(string email);
    Task<Result<object>> RefreshToken(string token);
}
