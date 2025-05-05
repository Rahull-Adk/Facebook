using System.ComponentModel.DataAnnotations;

namespace Facebook.DTOs;

public record class UpdateProfileDto(
    string? FirstName,
    string? LastName,
    string? CurrentPassword,
    string? NewPassword,
    string? ConfirmNewPassword,
    IFormFile? Image,
    DateTime? Birthday
    );
