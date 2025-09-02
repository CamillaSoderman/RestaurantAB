namespace RestaurantAB.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime StartTime { get; set; }
        public int NumberOfGuests { get; set; }

    }
}
