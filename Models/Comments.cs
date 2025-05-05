using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Models;

public class Comments
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid ByUserId { get; set; }
    public string Content { get; set; }
    [ForeignKey("PostId")]
    public Posts Post { get; set; }
    [ForeignKey("ByUserId")]
    public User ByUser { get; set; }
}