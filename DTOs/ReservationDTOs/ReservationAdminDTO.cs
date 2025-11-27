namespace RestaurantAB.DTOs.ReservationDTOs
{
    // Admin view
    public class ReservationAdminDTO
    {
        public int ResId { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }  // 2 hour is implemented
        public int NumberOfGuests { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
