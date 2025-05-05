namespace Facebook.DTOs;

public record FriendDto(Guid userId, Guid friendId, DateTime createdAt);
