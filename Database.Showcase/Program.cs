// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public List<Wallet> Wallets { get; set; }
}

public class Wallet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
    public decimal Balance { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }
}

public class ShowcaseContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=./database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.HasMany(a => a.Wallets).WithOne(a => a.Person);
        });
    }
}