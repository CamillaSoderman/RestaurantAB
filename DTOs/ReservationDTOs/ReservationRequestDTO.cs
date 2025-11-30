using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAB.DTOs.ReservationDTOs
{
    public class ReservationRequestDTO
    {
        // Customer information
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        // Booking information
        public int TableId { get; set; }
        public DateTime StartTime { get; set; }
        public int Guests { get; set; }
    }
}
