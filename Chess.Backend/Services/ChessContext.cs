using Chess.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Chess.Backend.Services;

public class ChessContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lobby> Lobbies { get; set; }
    public DbSet<Game> Games { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
        });

        modelBuilder.Entity<Lobby>(builder => builder.HasKey(l => l.Id));
        
        
        modelBuilder.Entity<Game>(builder =>
        {
            builder.HasKey(g => g.Id);
        });
    }
}