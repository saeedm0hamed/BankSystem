using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Display(Name = "Open Date")]
        public DateOnly OpenDate { get; init; }

        public virtual ICollection<Employee>? Employees { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Location}";
        }
    }
}
