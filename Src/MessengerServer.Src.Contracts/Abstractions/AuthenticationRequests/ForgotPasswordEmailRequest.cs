namespace MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;

public class ForgotPasswordRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
