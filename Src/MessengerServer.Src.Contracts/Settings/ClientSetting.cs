namespace MessengerServer.Src.Contracts.Settings;

public class ClientSetting
{
    public const string SectionName = "ClientSetting";
    public required string ClientUrl { get; set; }
    public required string ClientActiveAccount { get; set;}
    public required string ClientActiveUpdateEmail { get; set;}
}
