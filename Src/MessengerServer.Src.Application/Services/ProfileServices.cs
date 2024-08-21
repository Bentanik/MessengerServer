using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;

namespace MessengerServer.Src.Application.Services;

public class ProfileServices(IUnitOfWork unitOfWork) : IProfileServices
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<object>> GetProfileUser(string email)
    {
        var result = await _unitOfWork.UserRepository.GetProfileUser(email);
        return new Result<object>
        {
            Error = result != null ? 0 : 1,
            Data = result
        };
    }
}
