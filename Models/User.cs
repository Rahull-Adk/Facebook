using System.ComponentModel.DataAnnotations;

namespace Facebook.Models;

public class User
{
    public Guid Id { get; set; }

    [MinLength(4, ErrorMessage = "Minimum Username length is 4"), MaxLength(20, ErrorMessage = "Maximum Username length is 30")]
    public required string Username { get; set; }

    [MinLength(2, ErrorMessage = "Minimum FirstName length is 2"), MaxLength(20, ErrorMessage = "Maximum FirstName length is 30")]
    public required string FirstName { get; set; }

    [MinLength(4, ErrorMessage = "Minimum LastName length is 3"), MaxLength(20, ErrorMessage = "Maximum LastName length is 30")]
    public required string LastName { get; set; }

    [EmailAddress]
    [MinLength(5), MaxLength(50)]
    public required string Email { get; set; }

    [MinLength(6)]
    public required string Password { get; set; }

    [MinLength(4, ErrorMessage = "Minimum URL length is 4")]
    public string? ProfilePicture { get; set; } = "https://i.pravatar.cc/300";

    public bool IsDeleted { get; set; } = false;
    public DateTime? Birthday { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public List<Friends> Friends { get; set; } = new List<Friends>();
    public List<SendFriendRequest> SendFriendRequests { get; set; } = new List<SendFriendRequest>();
}