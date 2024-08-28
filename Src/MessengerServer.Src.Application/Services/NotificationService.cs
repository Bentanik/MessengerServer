using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.MapExtensions.NotificationMapExtensions;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Application.Services;

public class NotificationService(IUnitOfWork unitOfWork, IHubNotificationService hubNotificationService)
    : INotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IHubNotificationService _hubNotificationService = hubNotificationService;
    public async Task PushNotificationFriendService(ViewUserAddedFriendDTO viewUserAddedFriendDto, AddNotificationFriendDTO request)
    {
        var isExitNotificationFriend = await _unitOfWork.NotificationAddFriendRepository
            .IsExistsNotificationFriendAsync(request.FromUserId, request.ToUserId);

        if(isExitNotificationFriend)
        {
            return;
        }

        var mapper = request.ToNotificationAddFriend();
        mapper.Status = false;
        _unitOfWork.NotificationAddFriendRepository.Add(mapper);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
        {
            return;
        }
        await _hubNotificationService.SendNotificationFriendAsync(request.ToUserId, viewUserAddedFriendDto);
        return;
    }
    public async Task CountNotificationService(Guid userId)
    {
        int countId = await _unitOfWork.NotificationAddFriendRepository
            .CountNotificationAddedFriendByStatusAsync(userId);
        await _hubNotificationService.CountNotificationAsync(userId, countId);
    }
    public async Task<Result<object>> GetTwoNotificationFriendService(Guid userId)
    {
        var reuslt = await _unitOfWork.NotificationAddFriendRepository.GetTwoInfoUserNotificationFriendAsync(userId);
        return new Result<object>
        {
            Error = 0,
            Data = reuslt
        };
    }
}
