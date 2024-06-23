using FinanceTracker.Components.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services;

public class UserService
{
    private readonly AppDbContext _context;
    private readonly CustomAuthStateProvider _authStateProvider;

    public UserService(AppDbContext context, CustomAuthStateProvider authStateProvider)
    {
        _context = context;
        _authStateProvider = authStateProvider;
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

    public async Task<User> Login(User user)
    {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login && u.Password == user.Password);
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task LogoutAsync()
        {
            await _authStateProvider.LogoutAsync();
        }

}