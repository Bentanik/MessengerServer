namespace MessengerServer.Src.Contracts.Settings;
public class RedisSetting
{
    public const string SectionName = "RedisSetting";
    public required string DefaultConnection { get; set; }
}