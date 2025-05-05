namespace Facebook.DTOs;

public record CommentResponseDto(Guid Id, Guid PostId, Guid ByUserId, string content);