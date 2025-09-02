using Microsoft.EntityFrameworkCore;
using RestaurantAB.Models;

namespace RestaurantAB.Data
{
    public class RestaurantABDbContext : DbContext
    {
        public RestaurantABDbContext(DbContextOptions<RestaurantABDbContext> options) 
            : base(options)  { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Seed 1 admin
            //modelBuilder.Entity<Admin>().HasData(new Admin
            //{
            //    Id = 1,
            //    Username = "admin",
            //    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!")
            //});

            // Seed Table
            modelBuilder.Entity<Table>().HasData(
                new Table { Id = 1, TableNumber = 1, Capacity = 2 },
                new Table { Id = 2, TableNumber = 2, Capacity = 4 },
                new Table { Id = 3, TableNumber = 3, Capacity = 6 }
            );

            // Seed meny
            modelBuilder.Entity <Menu>().HasData(
                new Menu
                {
                    Id = 1,
                    Name = "Cheeseburger",
                    Description = "Saftig burgare med ost",
                    Price = 99.0m,
                    IsPopular = true,
                    ImageUrl = "https://example.com/cheeseburger.jpg"
                },
                new Menu
                {
                    Id = 2,
                    Name = "Caesarsallad",
                    Description = "Fräsch sallad med kyckling och parmesan",
                    Price = 85.0m,
                    IsPopular = false,
                    ImageUrl = "https://example.com/caesarsalad.jpg"
                },
                new Menu
                {
                    Id = 3,
                    Name = "Chokladtårta",
                    Description = "Kladdig chokladtårta med grädde",
                    Price = 55.0m,
                    IsPopular = true,
                    ImageUrl = "https://example.com/chokladtarta.jpg"
                }
            );
        }

    }


}
