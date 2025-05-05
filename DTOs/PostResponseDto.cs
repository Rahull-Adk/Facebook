using Facebook.Models;

namespace Facebook.DTOs;

public record PostResponseDto(Guid Id, Guid UserId, string Title, string Content,string ImageUrl,int Likes, IEnumerable<CommentResponseDto> Comments, DateTime Created, DateTime Updated);
