using Chess.Backend.Models;
using Chess.Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace Chess.Backend.Repositories;

public class UserRepository
{
    private readonly ChestContext _context;

    public UserRepository(ChestContext context)
    {
        _context = context;
    }
    public async Task CreateUser(User usermodel)
    {
        _context.Users.Add(usermodel);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UserExists(string username)
    {
            return await _context.Users.AnyAsync(u => u.Name == username);
    }
}