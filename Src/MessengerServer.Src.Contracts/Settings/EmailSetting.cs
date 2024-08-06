namespace MessengerServer.Src.Contracts.Settings;

public class EmailSetting
{
    public const string SectionName = "EmailSetting";
    public required string EmailHost { get; set; }
    public required string EmailUsername { get; set; }
    public required string EmailPassword { get; set; }
}
