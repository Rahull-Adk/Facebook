using Facebook.DTOs;
using Facebook.Models;
using Facebook.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Controllers
{
    [Route($"api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService<User> _authService;

        public AuthController(IAuthService<User> authService)
        {
            _authService = authService;
        }
        
        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            
            var registeredUser = await _authService.RegisterService(request);
            if (!registeredUser.IsSuccess)
            {
                return BadRequest(registeredUser.Message);
            }
            return Ok(registeredUser.Data);
        }
        
        [HttpPost("/login")] 
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var token = await _authService.LoginService(request);

            if (!token.IsSuccess)
            {
                return BadRequest(token.Message);
            }
            return Ok(token);
        }

    }
}
