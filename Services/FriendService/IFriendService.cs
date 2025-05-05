using Facebook.DTOs;
using Facebook.Models;

namespace Facebook.Services.FriendService;

public interface IFriendService
{
    public Task<ApiResponse<string>> AddFriendService(Guid userId, Guid friendId);
    public Task<ApiResponse<string>> AcceptOrDeclineRequestService(Guid userId, Guid friendId, bool willAccept);
    public Task<ApiResponse<string>> UnfriendService(Guid userId, Guid friendId);
    public Task<ApiResponse<IEnumerable<Friends>>> GetAllFriends(Guid userId);

    public Task<ApiResponse<IEnumerable<SendFriendRequest>>> GetAllSendFriendRequests(Guid userId);
    public Task<ApiResponse<IEnumerable<SendFriendRequest>>> GetAllReceivedRequests(Guid userId);


}