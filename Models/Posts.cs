using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Models;

public class Posts
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters long."), MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public required string Title { get; set; }
    
    [MinLength(4, ErrorMessage = "Content must be at least 4 characters long."), MaxLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
    public string? Content { get; set; }
    
    [MaxLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
    public string? ImageUrl { get; set; }

    public int Likes { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Comments> Comment { get; set; } =  new List<Comments>();
    [ForeignKey("UserId")]
    public User User { get; set; }
    
}