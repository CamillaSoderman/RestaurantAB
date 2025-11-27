using System.ComponentModel.DataAnnotations;

namespace RestaurantAB.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }

        [MaxLength(20)]
        public string Role { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
