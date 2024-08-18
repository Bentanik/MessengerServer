namespace MessengerServer.Src.Domain.Entities;

public class User : BaseEntity
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Biography { get; set; }
}
