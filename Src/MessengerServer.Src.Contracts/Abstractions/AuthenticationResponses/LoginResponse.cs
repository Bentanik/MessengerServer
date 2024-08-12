using MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Contracts.Abstractions.AuthenticationResponses;

public class LoginResponse
{
    public ViewHeaderUserDTO? ViewHeaderUserDTO { get; set; }
    public LoginTokenDTO? LoginTokenDTO{ get; set; }
}
