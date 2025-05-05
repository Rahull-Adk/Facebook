using System.Runtime.CompilerServices;
using Facebook.DTOs;
using Facebook.Services.ImageService;
using Facebook.Services.PostService;
using Facebook.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Facebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IImagService _imagService;

        public PostController(IUserService userService, IPostService postService, IImagService imageService
        )
        {
            _userService = userService;
            _postService = postService;
            _imagService = imageService;
        }

        [Authorize]
        [HttpPost("/post")]
        public async Task<IActionResult> CreatePost([FromForm] PostRequestDto request)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUser(User);

                if (!currentUser.IsSuccess)
                {
                    return BadRequest(currentUser.Message);
                }

                var post = await _postService.CreatePost(currentUser.Data!, request);

                if (!post.IsSuccess)
                {
                    if (request.Image != null && request.Image.Length > 0)
                    {
                        await _imagService.DeleteImageAsync(post.Data!.ImageUrl);
                    }

                    return BadRequest(post.Message);
                }

                return Ok(post.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("/post/{id:guid}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            try
            {
                var result = await _postService.GetById(id);
                if (!result.IsSuccess) return BadRequest(result.Message);

                return Ok(result.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       
        [HttpGet("/post/")]
        public async Task<IActionResult> GetAllPost()
        {
            try
            {
                var result = await _postService.GetAllPosts();
                if (!result.IsSuccess) return BadRequest(result.Message);

                return Ok(result.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize]
        [HttpGet("/post/getMyPosts")]
        public async Task<IActionResult> GetMyPosts()
        {
            try
            {
                var currentUser = await _userService.GetCurrentUser(User);
                if (!currentUser.IsSuccess)
                {
                    return BadRequest(currentUser.Message);
                }

                var posts = await _postService.GetMyPosts(currentUser.Data!.Id);

                if (!posts.IsSuccess) return BadRequest(posts.Message);
                return Ok(posts.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("/post/getFeedContents/{page:int}")]
        public async Task<IActionResult> GetFeedContents()
        {
            try
            {
                var currentUser = await _userService.GetCurrentUser(User);
                if (!currentUser.IsSuccess)
                {
                    return BadRequest(currentUser.Message);
                }

                var feeds = await _postService.GetFeedContents(currentUser.Data!.Id, page: 1);

                if (!feeds.IsSuccess) return BadRequest(feeds.Message);
                return Ok(feeds.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("/post/{id:guid}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromForm] PostRequestDto request)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUser(User);
                if (!currentUser.IsSuccess)
                {
                    return BadRequest(currentUser.Message);
                }
                var result = await _postService.UpdatePost(currentUser.Data!, id ,request);
                
                if (!result.IsSuccess) return BadRequest(result.Message);
                return Ok(result.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize]
        [HttpDelete("/post/{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUser(User);
                if (!currentUser.IsSuccess)
                {
                    return BadRequest(currentUser.Message);
                }

                var result = await _postService.DeletePost(currentUser.Data, id);
                
                if (!result.IsSuccess) return BadRequest(result.Message);
                return Ok(result.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("/post/{id:guid}/likes")]
        public async Task<IActionResult> LikeUnlikePost(Guid id)
        {
            try
            {
                var user = await _userService.GetCurrentUser(User);
                if (!user.IsSuccess)
                {
                    return BadRequest(user.Message);
                }

                var result = await _postService.LikeUnlikePost(user.Data!.Id, id);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                
                return Ok(result.Data);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize]
        [HttpPost("/post/getAlllikes")]
        public async Task<IActionResult> GetLikedPost(Guid id)
        {
            try
            {
                var user = await _userService.GetCurrentUser(User);
                if (!user.IsSuccess)
                {
                    return BadRequest(user.Message);
                }

                var result = await _postService.GetLikedPosts(user.Data!.Id);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                
                return Ok(result.Data);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("/post/{id:guid}/comments")]
        public async Task<IActionResult> Comment(Guid id, [FromBody] CommentDto comment)
        {
            try
            {
                var user = await _userService.GetCurrentUser(User);
                if (!user.IsSuccess)
                {
                    return BadRequest(user.Message);
                }

                var result = await _postService.CommentPost(user.Data!.Id, id, comment.comment);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                
                return Ok(result.Data);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            } 
        }
        
        
    }
}

