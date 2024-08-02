using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

namespace MessengerServer.Src.Application.Interfaces;

public interface IAuthenticationServices
{
    Task<Result<object>> Register(RegisterDTO registerDto);
}
