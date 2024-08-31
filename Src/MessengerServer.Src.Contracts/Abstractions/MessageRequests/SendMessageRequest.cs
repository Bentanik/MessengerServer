namespace MessengerServer.Src.Contracts.Abstractions.MessageRequests;

public class SendMessageRequest
{
    public Guid UserInitId { get; set; }
    public Guid UserReceiveId { get; set; }
    public string Content { get; set; }
}
