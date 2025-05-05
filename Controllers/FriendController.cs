using Facebook.DTOs;
using Facebook.Services.FriendService;
using Facebook.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFriendService _friendService;
      

        public FriendController(IUserService userService, IFriendService friendService)
        {
            _userService = userService;
            _friendService = friendService;
        }
        [Authorize]
        [HttpPost("/friend/{friendId:guid}/add")]
        public async Task<IActionResult> AddFriend(Guid friendId)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUser(User);
                if (!currentUser.IsSuccess) return Unauthorized(currentUser.Message);
                var addFriend = await _friendService.AddFriendService(currentUser.Data!.Id, friendId);

                if (!addFriend.IsSuccess) return BadRequest(addFriend.Message);

                return Ok("Friend request sent.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("/friend/getSentRequests")]
        public async Task<IActionResult> SentRequests()
        {
            var currentUser = await _userService.GetCurrentUser(User);
            if (!currentUser.IsSuccess) return Unauthorized(currentUser.Message);
            
            var sentRequests = await _friendService.GetAllSendFriendRequests(currentUser.Data!.Id);
            
            if(!sentRequests.IsSuccess) return BadRequest(sentRequests.Message);

            if (!sentRequests.Data!.Any())
            {
                return Ok("No requests sent.");
            }

            var response = sentRequests.Data!.Select(req => new SentFriendDto(req.FromUserId, req.ToUserId, req.SentAt, req.Status, req.isDeleted));
       
            return Ok(response);
        }
        
        [Authorize]
        [HttpPost("/friend/getReceivedRequests")]
        public async Task<IActionResult> ReceivedRequests()
        {
            var currentUser = await _userService.GetCurrentUser(User);
            if (!currentUser.IsSuccess) return Unauthorized(currentUser.Message);
            
            var receivedRequest = await _friendService.GetAllReceivedRequests(currentUser.Data!.Id);
            
            if(!receivedRequest.IsSuccess) return BadRequest(receivedRequest.Message);

            if (!receivedRequest.Data!.Any())
            {
                return Ok("No requests received.");
            }

            var response = receivedRequest.Data!.Select(req => new SentFriendDto(req.FromUserId, req.ToUserId, req.SentAt, req.Status, req.isDeleted));
       
            return Ok(response);
        }

        [Authorize]
        [HttpPost("/friend/{friendId:guid}/acceptOrDecline")]
        public async Task<IActionResult> AcceptOrDecline(Guid friendId,[FromBody] bool willAccept)
        {
            var currentUser = await _userService.GetCurrentUser(User);
          
            if(!currentUser.IsSuccess) return BadRequest(currentUser.Message);
            
            var result = await _friendService.AcceptOrDeclineRequestService(currentUser.Data!.Id, friendId, willAccept);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            } 
            
            
            return willAccept ? Ok("Friend request accepted") : Ok("Friend request declined");
            
        }

        [Authorize]
        [HttpGet("/friend/getAllFriends")]
        public async Task<IActionResult> GetAllFriends()
        {
            var currentUser = await _userService.GetCurrentUser(User);
            if (!currentUser.IsSuccess) return Unauthorized(currentUser.Message);
            var results = await _friendService.GetAllFriends(currentUser.Data!.Id);
            if (!results.IsSuccess) return BadRequest(results.Message);
            if(!results.Data!.Any()) return Ok("No friends yet.");
            var response = results.Data!.Select(f => new FriendDto(f.UserId, f.FriendId, f.CreatedAt));
            return Ok(response);
        }

        [Authorize]
        [HttpPost("/friend/{friendId:guid}/unfriend")]
        public async Task<IActionResult> Unfriend(Guid friendId)
        {
            var currentUser = await _userService.GetCurrentUser(User);
            if (!currentUser.IsSuccess) return Unauthorized(currentUser.Message);
            var result = await _friendService.UnfriendService(currentUser.Data!.Id, friendId);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok("Unfriended successfully.");
        }
    }
}
