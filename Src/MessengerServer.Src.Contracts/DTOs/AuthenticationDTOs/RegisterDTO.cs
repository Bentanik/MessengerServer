namespace MessengerServer.Src.Contracts.DTOs.AuthenticationDTOs;

public class RegisterDTO
{
    public Guid? UserId { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FullName { get; set; }
}
