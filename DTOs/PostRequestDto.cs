namespace Facebook.DTOs;

public record PostRequestDto(
    string Title,
    string? Content,
    IFormFile? Image
    );
