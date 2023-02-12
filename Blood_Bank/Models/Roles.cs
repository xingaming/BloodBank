using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Blood_Bank.Models
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; }
        public string NormalizedName { set; get; }
        public string ConcurrencyStamp { set; get; }
    }
}
