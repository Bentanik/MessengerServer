namespace MessengerServer.Src.Contracts.Abstractions.MessageRequests;

public class GetMessageRequest
{
    public Guid UserInitId { get; set; }
    public Guid UserReceiveId { get; set; }
}
