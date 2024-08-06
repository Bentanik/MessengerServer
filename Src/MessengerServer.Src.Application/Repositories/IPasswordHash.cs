namespace MessengerServer.Src.Application.Repositories;

public interface IPasswordHash
{
    string HashPassword(string password);
    bool VeriyPassword(string password, string passwordHash);
}
