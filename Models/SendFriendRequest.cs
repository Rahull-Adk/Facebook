using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Models;

public class SendFriendRequest
{
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    
    public FriendStatus Status { get; set; } = FriendStatus.Pending; 
    public bool isDeleted { get; set; } = false;
    [ForeignKey("FromUserId")]
    public User FromUser { get; set; }
    
    [ForeignKey("ToUserId")]
    public User ToUser { get; set; }
}

public enum FriendStatus
{
    None,
    Pending,   
    Accepted,  
    Declined   
}
