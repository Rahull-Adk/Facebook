using Facebook.DTOs;
using Facebook.Models;
using Facebook.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;

namespace Facebook.Controllers
{
    [Route("/api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [Authorize]
        [HttpGet("/me")]
        public async Task<IActionResult> GetMe()
        {
            var currentUser = await _userService.GetCurrentUser(User);
            if (currentUser.Data != null) currentUser.Data.Password = string.Empty;
            return !currentUser!.IsSuccess ? BadRequest(currentUser.Message) : Ok(currentUser.Data!);
        }

        [HttpGet("/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsername(username);
                if(!user.IsSuccess) return BadRequest($"Error retrieving user: {user.Message}");
                user.Data!.Password = string.Empty;
                return Ok(user.Data!);
            }
            catch (Exception e)
            {
                return BadRequest($"Error retrieving user: {e.Message}");
            }
        }

        [Authorize]
        [HttpPut("/updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto request)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUser(User);
                if(!currentUser.IsSuccess) return BadRequest($"Error retrieving user: {currentUser.Message}");
                var updatedResult = await _userService.UpdateUserProfile(request, currentUser.Data!);
                if (!updatedResult.IsSuccess) return BadRequest(updatedResult.Message!);
                return Ok(updatedResult.Data!);
            }
            catch (Exception e)
            {
                return BadRequest($"Error updating user: {e.Message}");
            }
        }

        [Authorize]
        [HttpPut("/deleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var currentUser = await _userService.GetCurrentUser(User);
            if (currentUser.Data == null)
            {
                return BadRequest(currentUser.Message);
            }
            currentUser.Data.Password = string.Empty;
            var result = await _userService.DeleteUser(currentUser.Data!);
            if (!result.IsSuccess) return BadRequest(result.Message!);
            return Ok("Account deleted successfully.");
        }
        
    }
}






















