namespace RestaurantAB.DTOs
{
    public class ReservationRequestDTO
    {
        public int TableId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public string CustomerEmail { get; set; }
        public DateTime StartTime { get; set; }
        public int Guests { get; set; }
    }
}
