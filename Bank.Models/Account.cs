using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; } = "Active";
        public string Type { get; set; }
        public decimal Balance { get; set; } = 0;
        [Display(Name = "Open Date")]
        public DateOnly OpenDate { get; init; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; } = null!;

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch? Branch { get; set; } = null!;

        public virtual ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<Transaction>? FromTransactions { get; set; }
        public virtual ICollection<Transaction>? ToTransactions { get; set; }
    }
}
