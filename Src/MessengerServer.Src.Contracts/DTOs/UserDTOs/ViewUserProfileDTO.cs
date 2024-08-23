namespace MessengerServer.Src.Contracts.DTOs.UserDTOs;

public class ViewUserProfileDTO
{
    public Guid? UserId { get; set; }
    public string? FullName { get; set; }
    public string? CropCoverPhoto { get; set; }
    public string? CropAvatar { get; set; }
}
