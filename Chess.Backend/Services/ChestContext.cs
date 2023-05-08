using Chess.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Chess.Backend.Services;

public class ChestContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(a => a.Id);
        });
    }
}