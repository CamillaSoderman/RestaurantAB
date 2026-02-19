namespace RestaurantAB.DTOs.ReservationDTOs
{

    public class ReservationUpdateDTO
    {
        public int ResId { get; set; }
        public int TableId { get; set; }
        public DateTime StartTime { get; set; }
        public int NumberOfGuests { get; set; }
    }


}
