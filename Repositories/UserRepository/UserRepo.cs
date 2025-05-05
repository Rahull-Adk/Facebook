using System.IO.Compression;
using Facebook.Data;
using Facebook.Models;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repositories.UserRepository;

public class UserRepo : IUserRepo
{
    private readonly AppDbContext _context;

    public UserRepo(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> UserExists(string email, string username)
    {
        return await _context.Users.Where(u => u.IsDeleted == false).AsNoTracking().AnyAsync(u => u.Email == email || u.Username == username);
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByUsernameOrEmail(string usernameOrEmail)
    {
        return await _context.Users.Where(u => u.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync(u => u.Email == usernameOrEmail || u.Username == usernameOrEmail);
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users.Where(u => u.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.Where(u => u.IsDeleted == false).AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetById(Guid userId)
    {
        return await _context.Users.Where(u => u.IsDeleted == false).FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> UpdateUser(User user)
    {
         _context.Users.Update(user);
         await _context.SaveChangesAsync();
         return user;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var allUser = await _context.Users.AsNoTracking().Where(u => u.IsDeleted == false).ToListAsync();
        return allUser;
    }



    public async Task DeleteUser(User user)
    {
        user.IsDeleted = true;
        await UpdateUser(user);
    }

    public async Task<string> GetImageUrl(string username)
    {
        var user = await _context.Users.AsNoTracking().Where(u => u.Username == username && u.IsDeleted == false).FirstOrDefaultAsync();
        var imageUrl = user!.ProfilePicture;
        return imageUrl!;
    }
}