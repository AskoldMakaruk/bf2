// See https://aka.ms/new-console-template for more information

using DistributedLaba;
using Microsoft.EntityFrameworkCore;

var dbContext = new DistibutedContext();

namespace DistributedLaba
{
    public class DistibutedContext : DbContext
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>(builder => { builder.HasKey(a => a.Name); });

            modelBuilder.Entity<Employee>(builder =>
            {
                builder.HasKey(a => a.Id);
                builder.HasOne(a => a.Position).WithMany(a => a.Employees).HasForeignKey(a => a.PositionName);
            });

            modelBuilder.Entity<ProductType>(builder => { builder.HasKey(a => a.Code); });

            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(a => a.Id);
                builder.HasOne(a => a.ProductType).WithMany(a => a.Products).HasForeignKey(a => a.CodeType);
            });
        }
    }

    public class Position
    {
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PositionName { get; set; }
        public Position Position { get; set; }
    }

    public class ProductType
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string CodeType { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public bool IsOnSale { get; set; }

        public ProductType ProductType { get; set; }
    }
}