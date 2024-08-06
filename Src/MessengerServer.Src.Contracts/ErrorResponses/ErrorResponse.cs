namespace MessengerServer.Src.Contracts.ErrorResponses;

public class ErrorResponse
{
    public required string ErrorCode { get; set; }
    public required string ErrorMessage { get; set; }
}