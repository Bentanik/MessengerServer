using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

namespace MessengerServer.Src.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    public async Task<Result<object>> Register(RegisterDTO registerDto)
    {
        return new Result<object>
        {
            Error = 0,
            Message = "Success",
            Data = registerDto
        };
    }
}
