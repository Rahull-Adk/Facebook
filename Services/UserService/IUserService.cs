using System.Security.Claims;
using Facebook.DTOs;
using Facebook.Models;

namespace Facebook.Services.UserService;

public interface IUserService
{
    public Task<ApiResponse<User>> GetCurrentUser(ClaimsPrincipal userClaim);
    public Task<ApiResponse<User>> GetUserByUsername(string username);
    public Task<ApiResponse<User>> UpdateUserProfile(UpdateProfileDto newProfile, User user);
    public Task<ApiResponse<string>> DeleteUser(User user);
}