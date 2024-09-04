namespace MessengerServer.Src.Contracts.DTOs.MessageDTOs;

public class ViewTheseFriendsMessagesDTO
{
    public Guid PartnerId { get; set; }
    public string CropAvatar { get; set; }
    public string Content { get; set; }
    public string FullName { get; set; }
}
