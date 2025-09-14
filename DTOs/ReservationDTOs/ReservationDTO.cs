namespace RestaurantAB.DTOs.ReservationDTOs
{
    // Customer view
    public class ReservationDTO
    {
        public int ResId { get; set; }
        public int TableId { get; set; }

        public int NumberOfGuests { get; set; }

        public DateTime StartTime { get; set; }
      
        public string CustomerName { get; set; }



    }
}
