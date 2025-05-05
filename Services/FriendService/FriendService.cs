using Facebook.DTOs;
 using Facebook.Models;
 using Facebook.Repositories.FriendRepository;
 using Facebook.Repositories.UserRepository;

 namespace Facebook.Services.FriendService;
 
 public class FriendService : IFriendService
 {
     private readonly IFriendRepo _friendRepo;
     private readonly IUserRepo _userRepo;
 
     public FriendService(IFriendRepo friendRepo, IUserRepo userRepo)
     {
         
         _friendRepo = friendRepo;
         _userRepo = userRepo;
     }
     public async Task<ApiResponse<string>> AddFriendService(Guid userId, Guid friendId)
     {
        
         var isFriendShipExist = await _friendRepo.IsRequestExist(userId, friendId);
 
         if (isFriendShipExist != null)
         {
             return new ApiResponse<string>("Friend request already exists");
         }
         
         var friendRequest = new SendFriendRequest()
         {
             FromUserId = userId,
             ToUserId = friendId,
             Status = FriendStatus.Pending
         };
         
         await _friendRepo.AddFriendRequest(friendRequest);
         
         return new ApiResponse<string>(data: "Friend request sent successfully.");
     }
 
     public async Task<ApiResponse<string>> AcceptOrDeclineRequestService(Guid userId, Guid friendId, bool willAccept)
     {
         var friendRequest = await  _friendRepo.IsRequestExist(userId, friendId);
 
         if (friendRequest is null)
         {
             return new ApiResponse<string>("Friend request does not exist");
         }
         if (willAccept)
         {
             if(friendRequest.Status == FriendStatus.Accepted) return new ApiResponse<string>("Friend request already accepted");
             
             friendRequest.Status = FriendStatus.Accepted;
             friendRequest.isDeleted = true;
             await _friendRepo.UpdateFriendRequest(friendRequest);
             var newFriend1 = new Friends
             {
                 UserId = friendRequest.FromUserId,
                 FriendId = friendRequest.ToUserId,
             };
             var newFriend2 = new Friends()
             {
                 UserId = friendRequest.ToUserId,
                 FriendId = friendRequest.FromUserId,
             };
             friendRequest.isDeleted = true;
             await _friendRepo.AddFriend(newFriend1);
             await _friendRepo.AddFriend(newFriend2);
         }
         else
         {
             if(friendRequest.Status == FriendStatus.Declined) return new ApiResponse<string>("Friend request already declined");
             friendRequest.Status = FriendStatus.Declined;
             friendRequest.isDeleted = true;
             await _friendRepo.UpdateFriendRequest(friendRequest);
         }
         
         
         await _friendRepo.SaveChanges();
         return friendRequest.Status == FriendStatus.Accepted ? new ApiResponse<string>(data: "Friend request accepted successfully.") : new ApiResponse<string>(data: "Friend request declined.");
     }
 
     public async Task<ApiResponse<string>> UnfriendService(Guid userId, Guid friendId)
     {
        
         var friend1 = await _friendRepo.GetFriends(userId, friendId);
         if(friend1.Count != 2) return new ApiResponse<string>("Friend does not exist");
         await _friendRepo.RemoveFriend(friend1);
         await _friendRepo.SaveChanges();
         return new ApiResponse<string>(data: "Unfriended successfully.");
     }

     public async Task<ApiResponse<IEnumerable<Friends>>> GetAllFriends(Guid userId)
     {
         var friends = await _friendRepo.GetAllFriends(userId);
         return new ApiResponse<IEnumerable<Friends>>(friends);
     }
     
     public async Task<ApiResponse<IEnumerable<SendFriendRequest>>> GetAllSendFriendRequests(Guid userId)
     {
         var sentRequests = await _friendRepo.GetAllSendRequests(userId);
         return new ApiResponse<IEnumerable<SendFriendRequest>>(sentRequests);
     }

     public async Task<ApiResponse<IEnumerable<SendFriendRequest>>> GetAllReceivedRequests(Guid userId)
     {
         var receivedRequests =  await _friendRepo.GetAllReceivedRequests(userId);
         return new ApiResponse<IEnumerable<SendFriendRequest>>(receivedRequests);
     }
 }