using MessengerServer.Src.Application.Repositories;

namespace MessengerServer.Src.Infrastructure.Services;

public class PasswordHash : IPasswordHashService
{
    private readonly int workFactor = 13; 
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor);
    }

    public bool VeriyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
}
