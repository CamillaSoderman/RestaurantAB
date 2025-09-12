

namespace RestaurantAB.DTOs
{
    public class ReservationDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        
        public int TableId { get; set; }

        public int NumberOfGuests { get; set; }

        public DateTime StartTime { get; set; }

       
    
    }
}
