using Facebook.DTOs;
using Facebook.Models;

namespace Facebook.Services.PostService;

public interface IPostService
{
    public Task<ApiResponse<PostResponseDto>> CreatePost(User currentUser, PostRequestDto postRequestDto);
    public Task<ApiResponse<PostResponseDto>> UpdatePost(User currentUser, Guid postId, PostRequestDto postRes);
    public Task<ApiResponse<string>> DeletePost(User currentUser, Guid postId);
    public Task<ApiResponse<PostResponseDto>> GetById(Guid id);
    public Task<ApiResponse<IEnumerable<PostResponseDto>>> GetMyPosts(Guid userId);
    public Task<ApiResponse<IEnumerable<PostResponseDto>>> GetFeedContents(Guid userId, int page);
    public Task<ApiResponse<string>> LikeUnlikePost(Guid userId, Guid postId);
    public Task<ApiResponse<IEnumerable<PostResponseDto>>> GetLikedPosts(Guid userId);
    public Task<ApiResponse<IEnumerable<PostResponseDto>?>> GetAllPosts();
    public Task<ApiResponse<PostResponseDto>> CommentPost(Guid userId, Guid postId, string comment);
}