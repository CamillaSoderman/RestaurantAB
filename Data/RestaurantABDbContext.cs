using Microsoft.EntityFrameworkCore;
using RestaurantAB.Data;
using RestaurantAB.Models;

public class RestaurantABDbContext : DbContext
{
    public RestaurantABDbContext(DbContextOptions<RestaurantABDbContext> options)
        : base(options) { }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Table> Tables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        DataBaseSeederAdmin.Seed(modelBuilder);

        // -------------------- MENU --------------------
        modelBuilder.Entity<Menu>()
            .Property(m => m.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Menu>().HasData(
            new Menu { Id = 1, Name = "Pizza Margherita", Price = 95.00m, IsPopular = false },
            new Menu { Id = 2, Name = "Pasta Carbonara", Price = 110.00m, IsPopular = false },
            new Menu { Id = 3, Name = "Caesar Salad", Price = 85.00m, IsPopular = false },
            new Menu { Id = 4, Name = "Kebab", Price = 95.00m, IsPopular = true },
            new Menu { Id = 5, Name = "Banana split", Price = 110.00m, IsPopular = true },
            new Menu { Id = 6, Name = "Pie", Price = 85.00m, IsPopular = true }
        );

        // -------------------- CUSTOMERS --------------------
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, CustomerName = "Anna Svensson", CustomerPhone = "0701234567", CustomerEmail = "anna@example.com" },
            new Customer { CustomerId = 2, CustomerName = "Erik Johansson", CustomerPhone = "0707654321", CustomerEmail = "erik@example.com" }
        );

        // -------------------- TABLES --------------------
        modelBuilder.Entity<Table>().HasData(
            new Table { TableId = 1, Capacity = 2 },
            new Table { TableId = 2, Capacity = 4 },
            new Table { TableId = 3, Capacity = 6 }
        );


    }
}
