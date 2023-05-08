using Database.Showcase;
using loxusing Microsoft.EntityFrameworkCore;

var context = new ShowcaseContext();

context.Database.EnsureCreated();

context.People.Add(new Person()
{
    Age = 10,
    Name = "Тимур",
    Wallets = new List<Wallet>()
    {
        new Wallet()
        {
            Accounts = new List<Account>()
            {
                new Account()
                {
                    Balance = -100_000,
                    Currency =
                        new Currency()
                        {
                            Token = "UAH"
                        },
                }
            },

            Name = "Кредит монобанк",
        }
    },
    Email = "pidor@loh.com"
});

context.SaveChanges();

namespace Database.Showcase
{
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

        public List<Account> Accounts { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }

    public class Account
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }

        public decimal Balance { get; set; }
    }

    public class Currency
    {
        public string Token { get; set; }
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

            modelBuilder.Entity<Currency>(builder => { builder.HasKey(a => a.Token); });
        }
    }
}