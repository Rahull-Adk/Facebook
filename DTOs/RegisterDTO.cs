using System.ComponentModel.DataAnnotations;

namespace Facebook.DTOs;

public record class RegisterDto(
    [Required] string UserName,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] string ConfirmPassword);