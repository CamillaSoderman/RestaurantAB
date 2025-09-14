using System.ComponentModel.DataAnnotations;

namespace RestaurantAB.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public string CustomerEmail { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
