using Microsoft.EntityFrameworkCore;

namespace RestaurantAB.Data
{
    public static class DataBaseSeederAdmin
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            //var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");

            //modelBuilder.Entity<Admin>().HasData(
            //    new Admin
            //    {
            //        Id = 1,
            //        Username = "SuperAdmin",
            //        Email = "admin@test.com",
            //        PasswordHash = passwordHash,
            //        Role = "Admin"
            //    }
            //);
        }
    }
}
