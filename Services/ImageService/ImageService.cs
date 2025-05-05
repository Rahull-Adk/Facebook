using CloudinaryDotNet;
using dotenv.net;
using Facebook.DTOs;
using CloudinaryDotNet.Actions;
namespace Facebook.Services.ImageService;

public class ImageService : IImagService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<ApiResponse<string?>> UploadImageAsync(IFormFile image)
    {
        try
        {
            if (image is null || image.Length == 0) return null;

            await using var stream = image.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.FileName, stream),
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            var uploadedUrl = uploadResult.SecureUrl?.ToString();
            return new ApiResponse<string?>(data: uploadedUrl);
        }
        catch (Exception e)
        {
            return new ApiResponse<string?>($"Error uploading image: {e}");
        }
    }

    public async Task<bool> DeleteImageAsync(string imgUrl)
    {
        string publicId = imgUrl.Split("/").Last().Split("/").First().Split(".")[0];
        if(string.IsNullOrEmpty(publicId)) return false;
        
        var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId
        ));
        Console.WriteLine(result);
        return result.Result == "ok";
        
    }
}