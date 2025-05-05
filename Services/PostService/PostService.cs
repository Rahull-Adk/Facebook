using Facebook.DTOs;
using Facebook.Models;
using Facebook.Repositories.PostRepository;
using Facebook.Services.ImageService;

namespace Facebook.Services.PostService;

public class PostService : IPostService
{
    private readonly IPostRepo _postRepo;
    private readonly IImagService _imageService;

    public PostService(IPostRepo postRepo, IImagService imageService)
    {
        _postRepo = postRepo;
        _imageService = imageService;
    }

    public async Task<ApiResponse<PostResponseDto>> CreatePost(User currentUser, PostRequestDto postRequestDto)
    {
        try
        {
            var post = new Posts()
            {
                Content = postRequestDto.Content,
                Title = postRequestDto.Title,
                ImageUrl = "",
                UserId = currentUser.Id,
                Likes = 0,
                CreatedAt = DateTime.UtcNow,
            };
            if (postRequestDto.Image != null)
            {
                var imageUploadResult = await _imageService.UploadImageAsync(postRequestDto.Image);
                if (!imageUploadResult.IsSuccess)
                {
                    return new ApiResponse<PostResponseDto>(imageUploadResult.Message!);
                }

                post.ImageUrl = imageUploadResult.Data;
            }


            var createdPost = await _postRepo.CreatePost(post);
            
            var comment = createdPost.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content));
            
            var response = new PostResponseDto(createdPost.Id, createdPost.UserId, createdPost.Title,
                createdPost.Content!, createdPost.ImageUrl!, createdPost.Likes, comment,
                createdPost.CreatedAt, DateTime.UtcNow);

            return new ApiResponse<PostResponseDto>(response);
        }
        catch (Exception e)
        {
            return new ApiResponse<PostResponseDto>(e.Message);
        }
    }

    public async Task<ApiResponse<PostResponseDto>> UpdatePost(User currentUser, Guid postId, PostRequestDto postRes)
    {
        try
        {
            var post = await _postRepo.GetPostById(postId);
            if (post is null)
            {
                return new ApiResponse<PostResponseDto>($"Post with id: {postId} was not found");
            }

            if (post.UserId != currentUser.Id)
            {
                return new ApiResponse<PostResponseDto>("Unauthorized");
            }

            if (!string.IsNullOrEmpty(postRes.Title)) post.Title = postRes.Title;
            if (!string.IsNullOrEmpty(postRes.Content)) post.Content = postRes.Content;
            if (postRes.Image != null)
            {
                await _imageService.DeleteImageAsync(post.ImageUrl!);
                var result = await _imageService.UploadImageAsync(postRes.Image);
                if (!result.IsSuccess)
                {
                    return new ApiResponse<PostResponseDto>(result.Message!);
                }

                post.ImageUrl = result.Data;
            }

            await _postRepo.UpdatePostAsync(post);
            var comment = post.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content));
            var returnPost = new PostResponseDto(post.Id,post.UserId, post.Title,
            post.Content!, post.ImageUrl!, post.Likes, comment, post.CreatedAt,
               DateTime.UtcNow);
            return new ApiResponse<PostResponseDto>(returnPost);
        }
        catch (Exception e)
        {
            return new ApiResponse<PostResponseDto>(e.Message);
        }
    }

    public async Task<ApiResponse<string>> DeletePost(User currentUser, Guid postId)
    {
        try
        {
            var post = await _postRepo.GetPostById(postId);
            if (post is null)
            {
                return new ApiResponse<string>($"Post with id: {postId} was not found");
            }

            if (post.UserId != currentUser.Id)
            {
                return new ApiResponse<string>("Unauthorized");
            }

            await _postRepo.DeletePost(post);

            return new ApiResponse<string>(data: "Post deleted successfully");
        }
        catch (Exception e)
        {
            return new ApiResponse<string>(e.Message);
        }
    }


    public async Task<ApiResponse<PostResponseDto>> GetById(Guid id)
    {
        try
        {
            var result = await _postRepo.GetPostById(id);
            if (result is null)
            {
                return new ApiResponse<PostResponseDto>($"No post found");
            }

            var comment = result.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content));
            var post = new PostResponseDto(
                Id: result.Id,
                UserId: result.UserId,
                Title: result.Title,
                Content: result.Content,
                ImageUrl: result.ImageUrl,
                Likes: result.Likes,
                Comments: comment,
                Created: result.CreatedAt,
                Updated: DateTime.UtcNow
            );
            return new ApiResponse<PostResponseDto>(post);
        }
        catch (Exception e)
        {
            return new ApiResponse<PostResponseDto>(e.Message);
        }
    }
    
    public async Task<ApiResponse<IEnumerable<PostResponseDto>?>> GetAllPosts()
    {
        try
        {
            var result = await _postRepo.GetAllPosts();
            if (result is null)
            {
                return new ApiResponse<IEnumerable<PostResponseDto>?>($"No post found");
            }
           
            var response = result.Select(r => new PostResponseDto(
                Id: r.Id,
                UserId: r.UserId,
                Title: r.Title,
                Content: r.Content,
                ImageUrl: r.ImageUrl,
                Likes: r.Likes,
                Comments: r.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content)),
                Created: r.CreatedAt,
                Updated: DateTime.UtcNow
            ));
            return new ApiResponse<IEnumerable<PostResponseDto>?>(response);
        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<PostResponseDto>?>(e.Message);
        }
    }

    public async Task<ApiResponse<PostResponseDto>> CommentPost(Guid userId, Guid postId, string comment)
    {
        try
        {
            var commentTable = new Comments
            {
                ByUserId = userId,
                Content = comment,
                PostId = postId
            };
            
            var result = await _postRepo.AddComment(commentTable);
            
            var response = new PostResponseDto(Id: result.Id, UserId: result.UserId, Title: result.Title, Content: result.Content, ImageUrl: result.ImageUrl, Likes: result.Likes, Comments: result.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content)) , Created: result.CreatedAt, Updated: DateTime.UtcNow);
            return new ApiResponse<PostResponseDto>(response);
        }
        catch (Exception e)
        {
            return new ApiResponse<PostResponseDto>(e.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<PostResponseDto>>> GetMyPosts(Guid userId)
    {
        try
        {
            var result = await _postRepo.GetUserPosts(userId);
            if (result is null)
            {
                return new ApiResponse<IEnumerable<PostResponseDto>>($"No content posted yet");
            }

            var response = result.Select(p => new PostResponseDto(Id: p.Id, UserId: p.UserId, Title: p.Title,
                Content: p.Content, ImageUrl: p.ImageUrl, Likes: p.Likes, Comments:  p.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content)), Created: p.CreatedAt,
                Updated: DateTime.UtcNow));

            return new ApiResponse<IEnumerable<PostResponseDto>>(response);

        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<PostResponseDto>>(e.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<PostResponseDto>>> GetFeedContents(Guid userId, int page)
    {
        try
        {
            var feed = await _postRepo.GetFeed(userId, page);

            var response = feed.Select(f => new PostResponseDto(Id: f.Id, UserId: f.UserId, Title: f.Title,
                Content: f.Content, ImageUrl: f.ImageUrl, Likes: f.Likes, Comments: f.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content)),  Created: f.CreatedAt,
                Updated: DateTime.UtcNow));
            return new ApiResponse<IEnumerable<PostResponseDto>>(response);
        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<PostResponseDto>>(e.Message);
        }
    }

    public async Task<ApiResponse<string>> LikeUnlikePost(Guid userId, Guid postId)
    {
        try
        {
            var post = await _postRepo.GetPostById(postId);
            if (post is null)
            {
                return new ApiResponse<string>($"Post with id: {postId} was not found");
            }

            var isPostAlreadyLiked = await _postRepo.HasAlreadyLiked(userId, postId);

            if (!isPostAlreadyLiked)
            {
                await _postRepo.LikePost(userId, postId);
                return new ApiResponse<string>(data: "Post Liked successfully");
            }
            else
            {
                var likedPost = await _postRepo.GetLikePost(userId, postId);
                if (likedPost is null)
                {
                    return new ApiResponse<string>($"Post with id: {postId} was not liked");
                }
                await _postRepo.UnlikePost(likedPost);
                return new ApiResponse<string>(data: "Post unliked successfully");

            }
            
        }
        catch (Exception e)
        {
            return new ApiResponse<string>(e.Message);
        }
    }

    public async Task<ApiResponse<IEnumerable<PostResponseDto>>> GetLikedPosts(Guid userId)
    {
        try
        {
            var likedPosts = await _postRepo.GetLikedPosts(userId);
            if(likedPosts is null) return new ApiResponse<IEnumerable<PostResponseDto>>($"No post liked");

            var response =  likedPosts.Select(lp => new PostResponseDto(UserId: lp.UserId, Id: lp.Id,
                Title: lp.Title, Content: lp.Content, ImageUrl: lp.ImageUrl, Likes: lp.Likes, Comments: lp.Comment.Select(c => new CommentResponseDto(c.Id, c.PostId, c.ByUserId, c.Content)), Created: lp.CreatedAt, Updated: DateTime.UtcNow));
            
            return new ApiResponse<IEnumerable<PostResponseDto>>(response);
        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<PostResponseDto>>(e.Message);
        }
    }
}