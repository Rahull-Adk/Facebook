using Facebook.DTOs;

namespace Facebook.Services.ImageService;

public interface IImagService
{
    public Task<ApiResponse<string?>> UploadImageAsync(IFormFile file);
    public Task<bool> DeleteImageAsync(string imgUrl);
}