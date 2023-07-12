using Chess.Backend.Models;
using Chess.Backend.Services;
using Chess.Backend.Views;
using Microsoft.EntityFrameworkCore;

namespace Chess.Backend.Repositories;

public class UserRepository
{
    private readonly ChessContext _context;

    public UserRepository(ChessContext context)
    {
        _context = context;
    }
    public async Task CreateUser(User usermodel)
    {
        _context.Users.Add(usermodel);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> LoginUser(UserCreateView username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Name == username.Name && u.PasswordHash == username.PasswordHash);
    }
    

    public async Task<bool> UserExists(string username)
    {
            return await _context.Users.AnyAsync(u => u.Name == username);
    }

    public async Task<User?> GetUser(Guid userId)
    {
        return  await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}