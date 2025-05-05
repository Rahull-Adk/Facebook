using Microsoft.Build.Framework;
namespace Facebook.DTOs;

public record class LoginDTO(string usernameOrEmail, string password);
