using MessengerServer.Src.Application.Interfaces;
using MessengerServer.Src.Contracts.Abstractions;
using MessengerServer.Src.Contracts.ErrorResponses;
using MessengerServer.Src.Contracts.MessagesList;
using MessengerServer.Src.Domain.Entities;
using MessengerServer.Src.Domain.Enum;

namespace MessengerServer.Src.Application.Services;

public class UserServices(IUnitOfWork unitOfWork)
    : IUserServices
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<object>> AddFriendService(Guid userId1, Guid userId2)
    {
        var isExitFriend = await _unitOfWork.FriendshipRepository.IsExistFriend(userId1, userId2);

        if (isExitFriend)
        {
            return new Result<object>
            {
                Error = 0,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.AddFriendFail02.GetMessage().Code,
                           ErrorMessage = MessagesList.AddFriendFail02.GetMessage().Message
                       }
                    }
            };
        }

        var friendshipEntity = new FriendshipEntity
        {
            UserInitId = userId1,
            UserReceiveId = userId2,
            Status = FriendshipStatusEnum.Pending,
        };
        _unitOfWork.FriendshipRepository.Add(friendshipEntity);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result == 0)
        {
            return new Result<object>
            {
                Error = 0,
                Data = new List<ErrorResponse>
                    {
                       new()
                       {
                           ErrorCode = MessagesList.AddFriendFail.GetMessage().Code,
                           ErrorMessage = MessagesList.AddFriendFail.GetMessage().Message
                       }
                    }
            };
        }
        var userAddedFriend = await _unitOfWork.UserRepository.GetInfoUserAddedFriendByUserIdAsync(userId1);

        return new Result<object>
        {
            Error = 0,
            Message = MessagesList.AddFriendSuccessfully.GetMessage().Message,
            Data = userAddedFriend,
        };
    }
    public async Task<Result<object>> GetStatusFriendService(Guid userId1, Guid userId2)
    {
        var result = await _unitOfWork.FriendshipRepository.GetFriendshipDTOAsync(userId1, userId2);
        return new Result<object>
        {
            Error = result != null ? 0 : 1,
            Data = result,
        };
    }
    public async Task<Result<object>> AcceptFriendService(Guid userInitId, Guid userReceiveId)
    {
        var friendShip = await _unitOfWork.FriendshipRepository.GetFriendshipAsync(userInitId, userReceiveId);
        if (friendShip == null)
        {
            return new Result<object>
            {
                Error = 1,
            };
        }
        friendShip.Status = FriendshipStatusEnum.Accepted;
        var notificationFriend = await _unitOfWork.NotificationAddFriendRepository
            .GetNotificationAddFriendAsync(userInitId, userReceiveId);
        if (notificationFriend == null)
        {
            return new Result<object>
            {
                Error = 1,
            };
        }
        _unitOfWork.FriendshipRepository.Update(friendShip);
        _unitOfWork.NotificationAddFriendRepository.Remove(notificationFriend);
        var result = await _unitOfWork.SaveChangesAsync();
        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
        };
    }
    public async Task<Result<object>> RejectFriendService(Guid userInitId, Guid userReceiveId)
    {
        var friendShip = await _unitOfWork.FriendshipRepository.GetFriendshipAsync(userInitId, userReceiveId);
        if (friendShip == null)
        {
            return new Result<object>
            {
                Error = 1,
            };
        }
        var notificationFriend = await _unitOfWork.NotificationAddFriendRepository
            .GetNotificationAddFriendAsync(userInitId, userReceiveId);
        if (notificationFriend == null)
        {
            return new Result<object>
            {
                Error = 1,
            };
        }
        _unitOfWork.FriendshipRepository.Remove(friendShip);
        _unitOfWork.NotificationAddFriendRepository.Remove(notificationFriend);
        var result = await _unitOfWork.SaveChangesAsync();
        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
        };
    }
    public async Task<Result<object>> UnfriendService(Guid userInitId, Guid userReceiveId)
    {
        var friendShip = await _unitOfWork.FriendshipRepository.GetFriendshipAsync(userInitId, userReceiveId);
        if (friendShip == null)
        {
            return new Result<object>
            {
                Error = 1,
            };
        }
        _unitOfWork.FriendshipRepository.Remove(friendShip);
        var result = await _unitOfWork.SaveChangesAsync();
        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
        };
    }
}
