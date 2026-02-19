using Microsoft.EntityFrameworkCore;
using RestaurantAB.Models;

namespace RestaurantAB.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(RestaurantABDbContext context)
        {
            await context.Database.MigrateAsync();


            if (context.Menus.Any() || context.Tables.Any() || context.Reservations.Any())
                return;

            // -----------------------------
            // ⭐ 1. Seed Tables
            // -----------------------------
            var tables = new List<Table>
            {
                new Table { TableNumber = 1, Capacity = 2 },
                new Table { TableNumber = 2, Capacity = 4 },
                new Table { TableNumber = 3, Capacity = 6 },
                new Table { TableNumber = 4, Capacity = 2 },
                new Table { TableNumber = 5, Capacity = 4 }
            };

            context.Tables.AddRange(tables);
            await context.SaveChangesAsync();

            // -----------------------------
            // ⭐ 2. Seed Menu Items
            // -----------------------------
            var menus = new List<Menu>
            {
                // ⭐ Populära rätter
                new Menu
                {
                    Name = "Pizza Margherita",
                    Description = "Klassisk pizza med tomatsås, mozzarella och basilika.",
                    Price = 129,
                    IsPopular = true,
                    ImageUrl = "Images/margherita.png"
                },
                new Menu
                {
                    Name = "Pasta Carbonara",
                    Description = "Krämig pasta med pancetta, äggula och pecorino.",
                    Price = 149,
                    IsPopular = true,
                    ImageUrl = "Images/carbonara.png"
                },
                new Menu
                {
                    Name = "Caesar Sallad",
                    Description = "Romansallad, krutonger, parmesan och grillad kyckling.",
                    Price = 119,
                    IsPopular = true,
                    ImageUrl = "Images/ceasar.png"
                },

                // ⭐ Vanliga rätter
                new Menu
                {
                    Name = "Lasagne al Forno",
                    Description = "Husets lasagne med långkokt köttfärssås.",
                    Price = 139,
                    IsPopular = false,
                    ImageUrl = "Images/placeholder.svg"
                },
                new Menu
                {
                    Name = "Bruschetta",
                    Description = "Rostat bröd med tomat, basilika och olivolja.",
                    Price = 79,
                    IsPopular = false,
                    ImageUrl = "Images/placeholder.svg"
                },
                new Menu
                {
                    Name = "Tiramisu",
                    Description = "Klassisk italiensk dessert med mascarpone och espresso.",
                    Price = 89,
                    IsPopular = false,
                    ImageUrl = "Images/placeholder.svg"
                }
            };

            context.Menus.AddRange(menus);
            await context.SaveChangesAsync();




        }
    }
}
