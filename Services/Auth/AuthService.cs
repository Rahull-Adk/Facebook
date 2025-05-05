using System.IdentityModel.Tokens.Jwt;
using Facebook.Data;
using Facebook.DTOs;
using Facebook.Helpers;
using Facebook.Models;
using Facebook.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Facebook.Services.Auth;

public class AuthService : IAuthService<User>
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserRepo _userRepo;
    private readonly GenerateToken _generateToken;
    public AuthService(AppDbContext context, IPasswordHasher<User> passwordHasher, IUserRepo userRepo, GenerateToken generateToken)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _userRepo = userRepo;
        _generateToken = generateToken;
    }
    public async Task<ApiResponse<User>> RegisterService(RegisterDto request)
    {
        try
        {
            var isUserRegistered = await _userRepo.UserExists(request.Email, request.UserName);
            if (isUserRegistered)
            {
                return new ApiResponse<User>("Username or Email already exists.");
            }
            if (request.Password.Length < 6)
            {
                return new ApiResponse<User>("Password must be at least 6 characters long");
            }
            if (request.Password != request.ConfirmPassword)
            {
                return new ApiResponse<User>("Passwords do not match");
            }
            if (request.UserName.Length < 4)
            {
                return new ApiResponse<User>("Username must be at least 4 characters long");
            }
            if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
            {
                return new ApiResponse<User>("First name or last name is empty");
            }
            var user = new User
            {
                Username = request.UserName.ToLower(),
                Email = request.Email.ToLower(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
            };
            user.Password = _passwordHasher.HashPassword(user, request.Password);
            await _userRepo.AddUser(user);
            user.Password = string.Empty;
            return new ApiResponse<User>(user);

            
        }
        catch (Exception ex)
        {
            return new ApiResponse<User>(ex.Message);
        }
    }

    public async Task<ApiResponse<string>> LoginService(LoginDTO request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.usernameOrEmail) || string.IsNullOrEmpty(request.password))
            {
                return new ApiResponse<string>("All fields are required.");
            }

            var user = await _userRepo.GetByUsernameOrEmail(request.usernameOrEmail.ToLower());
            if (user is null)
            {
                return new ApiResponse<string>("User does not exist");
            }

            var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(user, user.Password, request.password);
            if (isPasswordCorrect == PasswordVerificationResult.Failed)
            {
                return new ApiResponse<string>("Invalid credentials.");
            }

            // Generate JWT Token

            var token = _generateToken.GenerateJwtToken(user);

            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<string>("Error: Token generation failed.");
            }

            return new ApiResponse<string>(data: token);

        }
        catch (Exception e)
        {
            return new ApiResponse<string>($"Error occured when generating token. {e.Message}");
        }
    }
}