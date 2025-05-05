using Facebook.Data;
using Facebook.Models;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repositories.FriendRepository;

public class FriendRepo : IFriendRepo
{
    private readonly AppDbContext _context;

    public FriendRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddFriendRequest(SendFriendRequest request)
    {
        await _context.SendFriendRequests.AddAsync(request);
        await _context.SaveChangesAsync();
    }

    public async Task AddFriend(Friends friend)
    {
        await _context.Friends.AddAsync(friend);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<SendFriendRequest?> IsRequestExist(Guid currentUserId, Guid friendId)
    {
        var existingRequest = await _context.SendFriendRequests.Where(r => r.isDeleted == false).FirstOrDefaultAsync(r => r.FromUserId == currentUserId && r.ToUserId == friendId || r.FromUserId == friendId && r.ToUserId == currentUserId);
        return existingRequest;
    }

    public async Task<SendFriendRequest?> GetFriendRequest(Guid userId, Guid friendId)
    {
        return await _context.SendFriendRequests.Where(r => r.isDeleted == false).FirstOrDefaultAsync(r => r.FromUserId == userId && r.ToUserId == friendId);
    }

    public async Task<List<Friends>> GetFriends(Guid id1, Guid id2)
    {
        return await _context.Friends.Where(f => (f.UserId == id1 && f.FriendId == id2) ||
                                                 (f.FriendId == id1 && f.UserId == id2)).ToListAsync();
    }

    public async Task RemoveFriend(List<Friends> friends)
    {
        _context.Friends.RemoveRange(friends);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRequest(SendFriendRequest request)
    {
        _context.SendFriendRequests.Remove(request);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Friends>> GetAllFriends(Guid userId)
    {
        return await _context.Friends.Where(f => f.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<SendFriendRequest>> GetAllSendRequests(Guid userId)
    {
        return await _context.SendFriendRequests.Where(r => r.FromUserId == userId && r.isDeleted == false).ToListAsync();
    }

    public async Task<IEnumerable<SendFriendRequest>> GetAllReceivedRequests(Guid userId)
    {
        return await _context.SendFriendRequests.Where(r => r.ToUserId == userId && r.isDeleted == false).ToListAsync();
    }

    public async Task UpdateFriendRequest(SendFriendRequest friendRequest)
    {
       _context.SendFriendRequests.Update(friendRequest);
        await _context.SaveChangesAsync();
    }
}
