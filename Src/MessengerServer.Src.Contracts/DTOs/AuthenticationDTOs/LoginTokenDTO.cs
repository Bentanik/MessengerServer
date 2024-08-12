namespace MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

public class LoginTokenDTO
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
