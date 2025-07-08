using Microsoft.EntityFrameworkCore;
using Bank.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bank.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.FromTransactions)
                .WithOne(t => t.FromAccount)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.ToTransactions)
                .WithOne(t => t.ToAccount)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            
        }
    }
}