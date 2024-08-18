namespace MessengerServer.Src.Contracts.DTOs.UserDTOs;

public class ViewUserProfilePrivateDTO
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public string? Biography { get; set; }
}
