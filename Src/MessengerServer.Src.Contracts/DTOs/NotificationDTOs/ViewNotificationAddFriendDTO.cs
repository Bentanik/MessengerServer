namespace MessengerServer.Src.Contracts.DTOs.NotificationDTOs;

public class ViewNotificationAddFriendDTO
{
    public Guid NotficiationAddFriendId { get; set; }
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? CropAvatar { get; set; }
}
