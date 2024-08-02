using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

namespace MessengerServer.Src.Application;

public static class MapExtensions
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
}
