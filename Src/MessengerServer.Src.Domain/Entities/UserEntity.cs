namespace MessengerServer.Src.Domain.Entities;

public class UserEntity : BaseEntity
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Biography { get; set; }
    public string? CropAvatar { get; set; }
    public string? FullAvatar { get; set; }
    public string? CropCoverPhoto { get; set; }
    public string? FullCoverPhoto { get; set; }
    public ICollection<FriendshipEntity>? FriendshipsInitiated { get; set; }
    public ICollection<FriendshipEntity>? FriendshipsReceived { get; set; }
    public ICollection<NotificationAddFriendEntitiy> SentNotificationAddFriends { get; set; }
    public ICollection<NotificationAddFriendEntitiy> ReceivedNotificationAddFriends { get; set; }
    public ICollection<MessageEntity> SentMessages { get; set; }
    public ICollection<MessageEntity> ReceivedMessages { get; set; }
}
