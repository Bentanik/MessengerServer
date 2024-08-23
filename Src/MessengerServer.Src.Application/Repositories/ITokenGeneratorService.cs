using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using System.Security.Claims;

namespace MessengerServer.Src.Application.Repositories;

public interface ITokenGeneratorService
{
    string GenerateToken(string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim>? claims = null);
    string GenerateAccessToken(UserGenerateTokenDTO user);
    string GenerateRefreshToken(UserGenerateTokenDTO user);
    string ValidateAndGetUserIdFromRefreshToken(string refreshToken);
}
