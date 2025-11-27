namespace RestaurantAB.DTOs.ReservationDTOs
{
    // Customer view
    public class ReservationDTO
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }   // 2 hour sitting is implemented
        public int NumberOfGuests { get; set; }
        public string CustomerName { get; set; } = string.Empty;
    }
}

