using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Models;

public class Friends
{
   
    public  Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required Guid FriendId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [ForeignKey("UserId")]
    public User User { get; set; }
    [ForeignKey("FriendId")]
    public User Friend { get; set; }

}