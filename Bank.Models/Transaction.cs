using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Type { get; set; } // "Withdrawal", "Deposit", "Transfer"

        // For withdrawals and deposits
        public int? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }

        // For transfers
        public int? FromAccountId { get; set; }
        [ForeignKey("FromAccountId")]
        public virtual Account? FromAccount { get; set; }

        public int? ToAccountId { get; set; }
        [ForeignKey("ToAccountId")]
        public virtual Account? ToAccount { get; set; }
    }
}