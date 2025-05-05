using Facebook.Models;

namespace Facebook.Repositories.PostRepository;

public interface IPostRepo
{
    public Task<Posts> CreatePost(Posts post);
    public Task<Posts?> GetPostById(Guid id);
    public Task UpdatePostAsync(Posts post);
    public Task DeletePost(Posts post);
    public Task<IEnumerable<Posts>?> GetUserPosts(Guid userId);
    public Task<IEnumerable<Posts>?> GetFeed(Guid userId, int page);
    public Task LikePost(Guid userId, Guid postId);
    public Task UnlikePost(Likes post);
    public Task<Likes?> GetLikePost(Guid userId, Guid postId);
    public Task<bool> HasAlreadyLiked(Guid userId, Guid postId);
    public Task<IEnumerable<Posts>?> GetLikedPosts(Guid userId);
    public Task<IEnumerable<Posts>?> GetAllPosts();
    public Task<Posts> AddComment(Comments comment);
}
