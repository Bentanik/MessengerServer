using MessengerServer.Src.Contracts.Abstractions;

namespace MessengerServer.Src.Application.Interfaces;

public interface IEditUserServices
{
    Task<Result<object>> GetProfilePrivate(string email);
    Task<Result<object>> UpdateEmailUser(string oldEmail, string newEmail);
    Task<Result<object>> ActiveUpdateEmailUser(string email);
    Task<Result<object>> UpdateFullNameUser(string email, string newFullName);
    Task<Result<object>> UpdateBiographyUser(string email, string biography);

}
