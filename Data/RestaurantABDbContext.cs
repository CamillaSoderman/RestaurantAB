using Microsoft.EntityFrameworkCore;
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


        modelBuilder.Entity<Menu>()
            .Property(m => m.Price)
            .HasPrecision(10, 2);


    }
}
