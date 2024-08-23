namespace MessengerServer.Src.Application.Repositories;

public interface IPasswordHashService
{
    string HashPassword(string password);
    bool VeriyPassword(string password, string passwordHash);
}
