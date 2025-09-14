using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAB.DTOs.ReservationDTOs
{
    public class ReservationRequestDTO
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }


        public int TableId { get; set; }
        public DateTime StartTime { get; set; }// Remove sekonds?
        public int Guests { get; set; }
    }
}
