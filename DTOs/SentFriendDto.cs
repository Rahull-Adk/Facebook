using Facebook.Models;

namespace Facebook.DTOs;

public record SentFriendDto(Guid FromUserId, Guid ToUserId, DateTime SentDateTime,FriendStatus Status,bool IsDeleted);