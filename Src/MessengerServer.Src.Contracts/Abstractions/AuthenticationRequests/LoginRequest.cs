namespace MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
