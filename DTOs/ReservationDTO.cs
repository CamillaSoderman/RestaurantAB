namespace RestaurantAB.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }

        // in customer DTO There is Name, PhoneNumber, Email
        public CustomerDTO Customer { get; set; }

        // in TableDTO There is TableId, TableNumber, Capacity
        public TableDTO Table { get; set; }

        public DateTime StartTime { get; set; }

       
    
    }
}
