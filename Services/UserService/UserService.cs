using System.Security.Claims;
using Facebook.DTOs;
using Facebook.Models;
using Facebook.Repositories.UserRepository;
using Facebook.Services.ImageService;
using Microsoft.AspNetCore.Identity;
namespace Facebook.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IImagService _imageService;
    public UserService(IUserRepo userRepo, IPasswordHasher<User> passwordHasher, IImagService imageService)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
        _imageService = imageService;
    }
    public async Task<ApiResponse<User>> GetCurrentUser(ClaimsPrincipal userClaim)
    {
        try
        {
            var userIdClaim = userClaim.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            if (userIdClaim is null)
            {
                return new ApiResponse<User>("User Id not found");
            }
            var user = await _userRepo.GetById(Guid.Parse(userIdClaim));
            return user is null ? new ApiResponse<User>("User not found") : new ApiResponse<User>(user);
        }
        catch (Exception ex)
        {
            return new ApiResponse<User>(ex.Message);
        }
        
    }

    public async Task<ApiResponse<User>> GetUserByUsername(string username)
    {
        try
        {
            var user = await _userRepo.GetByUsername(username);
            return user is null ? new ApiResponse<User>("User not found") : new ApiResponse<User>(user);
        }
        catch (Exception ex)
        {
            return new ApiResponse<User>(ex.Message);
        }
    }

    public async Task<ApiResponse<User>> UpdateUserProfile(UpdateProfileDto newProfile, User currentUser)
    {
        try
        {
            var loggedInUser = await _userRepo.GetById(currentUser.Id);

            if (loggedInUser is null) return new ApiResponse<User>("User not found");

            if (!string.IsNullOrEmpty(newProfile.FirstName))
            {
                loggedInUser.FirstName = newProfile.FirstName;
            }

            if (!string.IsNullOrEmpty(newProfile.LastName))
            {
                loggedInUser.LastName = newProfile.LastName;
            }
            if (!string.IsNullOrEmpty(newProfile.NewPassword))
            {
                if (string.IsNullOrEmpty(newProfile.CurrentPassword))
                {
                    return new ApiResponse<User>("Please enter your current password");
                }

                var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(loggedInUser,
                    loggedInUser!.Password, newProfile.CurrentPassword);
                if (string.IsNullOrEmpty(newProfile.ConfirmNewPassword))
                {
                    return new ApiResponse<User>("Please enter confirm password");
                }

                if (isPasswordCorrect is PasswordVerificationResult.Failed)
                {
                    return new ApiResponse<User>("Invalid credentials.");
                }
                var encryptedNewPassword = _passwordHasher.HashPassword(loggedInUser, newProfile.NewPassword);
                loggedInUser.Password = encryptedNewPassword;

                await _userRepo.UpdateUser(loggedInUser);
                loggedInUser.Password = string.Empty;
            }

            if (newProfile.Image is not null && newProfile.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(loggedInUser.ProfilePicture) &&
                    loggedInUser.ProfilePicture != "https://i.pravatar.cc/300")
                {
                    // get image from db
                    var extractedUrl = await _userRepo.GetImageUrl(loggedInUser.Username);
                    if (extractedUrl is null)
                    {
                        return new ApiResponse<User>("Image URL not found");
                    }

                    var deleteResult = await _imageService.DeleteImageAsync(extractedUrl);

                    if (!deleteResult)
                    {
                        return new ApiResponse<User>("Image deletion failed");
                    }

                }

                var result = await _imageService.UploadImageAsync(newProfile.Image);

                   if (!result.IsSuccess)
                   {
                       return new ApiResponse<User>(result.Message!);
                   }

                   loggedInUser.ProfilePicture = result.Data;
            }

            var updatedUser = await _userRepo.UpdateUser(loggedInUser);

            if (updatedUser is null)
            {
                return new ApiResponse<User>("Error updating user");
            }
            
            return new ApiResponse<User>(updatedUser!);
        }
        catch (Exception e)
        {
           return new ApiResponse<User>(e.Message);
        }
        
    }

    public async Task<ApiResponse<string>> DeleteUser(User user)
    {
        await _userRepo.DeleteUser(user);
        return new ApiResponse<string>("User deleted");
    }
}