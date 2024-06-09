using FinanceTracker.Components.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Register(string login, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Login == login))
            {
                return false; // Username already exists
            }

            var user = new User
            {
                Login = login,
                Password = password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
    }

    public async Task<User> Login(string login, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
    }

    public async Task<User> GetUserByLogin(string login)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }



}