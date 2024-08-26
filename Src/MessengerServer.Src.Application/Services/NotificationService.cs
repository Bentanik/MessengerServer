using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Application.MapExtensions.NotificationMapExtensions;
using MessengerServer.Src.Application.Repositories;
using MessengerServer.Src.Contracts.DTOs.NotificationDTOs;
using MessengerServer.Src.Contracts.DTOs.UserDTOs;

namespace MessengerServer.Src.Application.Services;

public class NotificationService(IUnitOfWork unitOfWork, IHubNotificationService hubNotificationService)
    : INotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IHubNotificationService _hubNotificationService = hubNotificationService;
    public async Task PushNotificationFriend(ViewUserAddedFriendDTO viewUserAddedFriendDto, AddNotificationFriendDTO request)
    {
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

    public async Task CountNotification(Guid userId)
    {
        int countId = await _unitOfWork.NotificationAddFriendRepository
            .CountNotificationAddedFriendByStatusAsync(userId);
        await _hubNotificationService.CountNotificationAsync(userId, countId);
    }
}
