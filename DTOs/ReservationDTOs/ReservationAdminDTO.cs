namespace RestaurantAB.DTOs.ReservationDTOs
{
    // Admin view
    public class ReservationAdminDTO : ReservationDTO
    {
        public int CustomerId { get; set; }       
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}
