using Facebook.Models;

namespace Facebook.Repositories.UserRepository;

public interface IUserRepo
{
    Task<bool> UserExists(string email, string username);
    Task AddUser(User user);
    Task<User?> GetByUsernameOrEmail(string usernameOrEmail);
    Task<User?> GetByUsername(string username);
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(Guid userId);
    Task<User?> UpdateUser(User user);
    Task<List<User>> GetAllUsers();
    Task DeleteUser(User user);

    Task<string> GetImageUrl(string username);
}