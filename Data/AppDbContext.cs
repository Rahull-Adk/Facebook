using Facebook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Facebook.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Friends> Friends { get; set; }
    public DbSet<Posts> Posts { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Likes> Likes { get; set; }
    public DbSet<SendFriendRequest> SendFriendRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Friends>()
            .HasOne(f => f.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Friends>()
            .HasOne(f => f.Friend)
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SendFriendRequest>()
            .HasOne(s => s.FromUser)
            .WithMany(u => u.
                SendFriendRequests)
            .HasForeignKey(s => s.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SendFriendRequest>()
            .HasOne(s => s.ToUser)
            .WithMany() 
            .HasForeignKey(s => s.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}