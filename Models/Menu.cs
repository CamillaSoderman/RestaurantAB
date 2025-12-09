namespace RestaurantAB.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPopular { get; set; }
        public string? ImageUrl { get; set; }

    }
}
