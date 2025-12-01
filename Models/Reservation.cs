using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAB.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TableId")]
        public int TableId { get; set; }
        public Table Table { get; set; }


        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        public int NumberOfGuests { get; set; }

        // New for accessCode for customer
        public string? AccessCode { get; set; }



    }
}
