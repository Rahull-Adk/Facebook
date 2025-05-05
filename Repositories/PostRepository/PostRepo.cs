using Facebook.Data;
using Facebook.Models;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repositories.PostRepository;

public class PostRepo : IPostRepo
{
    private readonly AppDbContext _context;
    public PostRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Posts> CreatePost(Posts post)
    {
        var createdPost = await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return createdPost.Entity;
    }

    public async Task<Posts?> GetPostById(Guid id)
    {
       return await _context.Posts.FindAsync(id); 
    }

    public async Task DeletePost(Posts post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Posts>?> GetUserPosts(Guid userId)
    {
        return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
    }
    
    public async Task UpdatePostAsync(Posts post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Posts>?> GetFeed(Guid userId, int page)
    {
       var friendsId = await _context.Friends.Where(f => f.UserId == userId).Select(f => f.FriendId).ToListAsync();

       var friendPosts = await _context.Posts.Where(p => friendsId.Contains(p.UserId)).OrderByDescending(p => p.CreatedAt)
           .Skip((page - 1) * 10)
           .Take(10)
           .ToListAsync();

       if (friendPosts.Count < 10)
       {
           var randomPosts = await _context.Posts.Where(p => !friendsId.Contains(p.UserId)).OrderBy(p => Guid.NewGuid()).Take(10 - friendPosts.Count).ToListAsync();
           
           var combinedPosts =  friendPosts.Union(randomPosts).Take(page).ToList();
           return combinedPosts;
       }
       return friendPosts;
    }

    public async Task LikePost(Guid userId, Guid postId)
    {
        var like = new Likes {PostId = postId, UserId = userId};
        await _context.Likes.AddAsync(like);
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        post.Likes += 1;
        await _context.SaveChangesAsync();
    }

    public async Task UnlikePost(Likes posts)
    { 
        _context.Likes.Remove(posts);
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == posts.PostId);
        post.Likes -= 1;
        await _context.SaveChangesAsync();
    }

    public async Task<Likes?> GetLikePost(Guid userId, Guid postId)
    {
        var likedPost = await _context.Likes.FirstOrDefaultAsync(p => p.UserId == userId && p.PostId == postId);
        return likedPost;
    }


    public async Task<bool> HasAlreadyLiked(Guid userId, Guid postId)
    {
        var alreadyLiked = await _context.Likes.AnyAsync(l => l.UserId == userId && l.PostId == postId);
        return alreadyLiked;
    }

    public async Task<IEnumerable<Posts>?> GetLikedPosts(Guid userId)
    {
        var likedPostIds = await _context.Likes.Where(l => l.UserId == userId).Select(l => l.PostId).ToListAsync();
        
        var likedPosts = await _context.Posts.Where(p => likedPostIds.Contains(p.Id)).OrderByDescending(p => p.CreatedAt).ToListAsync();
        
        return likedPosts;
        
    }

    public async Task<IEnumerable<Posts>?> GetAllPosts()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<Posts> AddComment(Comments comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        var post = await _context.Posts.Include(p => p.Comment).FirstOrDefaultAsync(p => p.Id == comment.PostId);
        return post!;
    }
}