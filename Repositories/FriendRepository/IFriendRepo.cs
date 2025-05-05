using Facebook.Data;
using Facebook.Models;

namespace Facebook.Repositories.FriendRepository;

public interface IFriendRepo
{
    public Task AddFriendRequest(SendFriendRequest request);
    public Task AddFriend(Friends friend);
    public Task SaveChanges();
    public Task<SendFriendRequest?> IsRequestExist(Guid currentUserId, Guid friendId);
    public Task<SendFriendRequest?> GetFriendRequest(Guid userId, Guid friendId);
    public Task<List<Friends>> GetFriends(Guid id1, Guid id2);
    public Task RemoveFriend(List<Friends> friend);
    public Task RemoveRequest(SendFriendRequest request);
    public Task UpdateFriendRequest(SendFriendRequest friendRequest);
    public Task<IEnumerable<Friends>> GetAllFriends(Guid userId);
    public Task<IEnumerable<SendFriendRequest>> GetAllSendRequests(Guid userId);
    
    public Task<IEnumerable<SendFriendRequest>> GetAllReceivedRequests(Guid userId);

}