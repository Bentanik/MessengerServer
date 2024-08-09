namespace MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

public class LoginDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
