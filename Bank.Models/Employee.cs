using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "National ID")]
        public string NationalID { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [ForeignKey("Branch")]
        public int? BranchId { get; set; }
        public virtual Branch? Branch { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} Name: {Name}";
        }
    }
}
