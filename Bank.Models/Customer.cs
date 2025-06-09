using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string NationalID { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string Email { get; set; }
        public ICollection<Account>? Accounts { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
