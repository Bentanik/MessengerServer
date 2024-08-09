using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;
using MessengerServer.Src.Domain.Entities;

namespace MessengerServer.Src.Application.MapExtensions.AuthenticationMapExtensions;

public static class AuthenticationMapExtension
{
    public static RegisterDTO ToRegisterDTO(this RegisterRequest registerRequest)
    {
        return new RegisterDTO
        {
            Email = registerRequest.Email,
            FullName = registerRequest.FullName,
            Password = registerRequest.Password,
        };
    }

    public static LoginDTO ToLoginDTO(this LoginRequest loginRequest)
    {
        return new LoginDTO
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password,
        };
    }

    public static User ToUser(this RegisterDTO registerDTO)
    {
        var user = new User
        {
            Email = registerDTO.Email,
            FullName = registerDTO.FullName,
            Password = registerDTO.Password,
        };
        if(registerDTO.UserId != null) user.Id = registerDTO.UserId.Value;
        return user;
    }
}
