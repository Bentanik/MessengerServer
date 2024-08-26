namespace MessengerServer.Src.Contracts.DTOs.UserDTOs;

public class ViewUserAddedFriendDTO
{
    public Guid? UserId { get; set; }
    public string? FullName { get; set; }
    public string? CropAvatar { get; set; }
}
