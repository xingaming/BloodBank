using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Bank.Models
{
    [Table("User")]
    public class User
    {     
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? Weight { get; set; }
        public int? BloodGroup { get; set; }
        public int Gender { get; set; }
        public DateTime Dob { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
