using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Models;

public class Likes
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Posts Post { get; set; }
    public Guid UserId { get; set; }
    public User ByUser { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}