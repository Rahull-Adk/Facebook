using Facebook.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Services.Auth;

public interface IAuthService<T>
{
    public Task<ApiResponse<T>> RegisterService(RegisterDto request);

    public Task<ApiResponse<string>> LoginService(LoginDTO request);

}