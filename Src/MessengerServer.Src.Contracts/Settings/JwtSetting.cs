namespace MessengerServer.Src.Contracts.Settings;

public class JwtSetting
{
    public const string SectionName = "JwtSetting";
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? AccessSecretToken { get; set; }
    public string? RefreshSecretToken { get; set; }
    public double AccessTokenExpMinute { get; set; }
    public double RefreshTokenExpMinute { get; set; }
}
