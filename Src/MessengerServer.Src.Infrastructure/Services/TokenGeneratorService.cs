using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;
using MessengerServer.Src.Contracts.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MessengerServer.Src.Infrastructure.Services;

public class TokenGeneratorService(IOptions<JwtSetting> jwtSetting) : ITokenGeneratorService
{
    private readonly JwtSetting _jwtSetting = jwtSetting.Value;

    public string GenerateToken(string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim>? claims = null)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(issuer, audience, claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(expirationMinutes), credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(UserGenerateTokenDTO user)
    {
        List<Claim> claims = new() {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleName)
        };
        if (_jwtSetting.AccessSecretToken != null && _jwtSetting.Issuer != null && _jwtSetting.Audience != null)
            return GenerateToken(_jwtSetting.AccessSecretToken, _jwtSetting.Issuer, _jwtSetting.Audience, _jwtSetting.AccessTokenExpMinute, claims);
        return null;
    }

    public string GenerateRefreshToken(UserGenerateTokenDTO user)
    {
        List<Claim> claims = new() {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.RoleName)
        };
        if (_jwtSetting.RefreshSecretToken != null && _jwtSetting.Issuer != null && _jwtSetting.Audience != null)
            return GenerateToken(_jwtSetting.RefreshSecretToken, _jwtSetting.Issuer, _jwtSetting.Audience, _jwtSetting.RefreshTokenExpMinute, claims);
        return null;
    }

    public string ValidateAndGetEmailFromToken(string refreshToken)
    {
        TokenValidationParameters validationParameters = new()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.RefreshSecretToken)),
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
        };

        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            return emailClaim?.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }

}
