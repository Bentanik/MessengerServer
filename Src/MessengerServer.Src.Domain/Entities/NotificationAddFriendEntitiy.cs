using MessengerServer.Src.Domain.Enum;

namespace MessengerServer.Src.Domain.Entities;

public class NotificationAddFriendEntitiy : BaseEntity
{
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public NotificationTypeEnum NotificationType { get; set; }
    public bool Status { get; set; } //Status is watch user have read notification?
}