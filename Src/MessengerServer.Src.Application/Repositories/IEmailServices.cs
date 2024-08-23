namespace MessengerServer.Src.Application.Repositories;

public interface IEmailServices
{
    Task<bool> SendMailAsync(string toEmail, string subject, string templateName, Dictionary<string, string> Body);
}
