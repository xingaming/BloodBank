using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Bank.Models
{
    [Table("BloodGroup")]
    public class BloodGroup
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
