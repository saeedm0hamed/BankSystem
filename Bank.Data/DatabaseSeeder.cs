using Bank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Data
{
    public static class DatabaseSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>()))
            {
                if (context.Branches.Any() || context.Customers.Any() || context.Employees.Any() || context.Accounts.Any())
                {
                    return;
                }

                var branches = new List<Branch>
                {
                    new Branch { Name = "Branch A", Location = "Arish", Phone = "3300000", OpenDate = DateOnly.FromDateTime(DateTime.Now)},
                    new Branch { Name = "Branch B", Location = "Cairo", Phone = "6600000", OpenDate = DateOnly.FromDateTime(DateTime.Now)},
                    new Branch { Name = "Branch C", Location = "Alexandria", Phone = "3800000", OpenDate = DateOnly.FromDateTime(DateTime.Now)}
                };
                context.Branches.AddRange(branches);
                context.SaveChanges();

                var customers = new List<Customer>
                {
                    new Customer {Name = "Customer A", NationalID = "31500000", Phone = "0100000000", Email = "customer.a@bank.com"},
                    new Customer {Name = "Customer B", NationalID = "31500000", Phone = "0110000000", Email = "customer.b@bank.com"},
                    new Customer {Name = "Customer C", NationalID = "31500000", Phone = "0120000000", Email = "customer.c@bank.com"}
                };
                context.Customers.AddRange(customers);
                context.SaveChanges();

                var employees = new List<Employee>
                {
                    new Employee {Name = "Employee A", NationalID = "31500000", Phone = "0100000000", Email = "employee.a@bank.com", Salary = 5000},
                    new Employee {Name = "Employee B", NationalID = "31500000", Phone = "0110000000", Email = "employee.b@bank.com", Salary = 5000},
                    new Employee {Name = "Employee C", NationalID = "31500000", Phone = "0120000000", Email = "employee.c@bank.com", Salary = 5000}
                };
                context.Employees.AddRange(employees);
                context.SaveChanges();

                var accounts = new List<Account>
                { 
                    new Account {Status = "Active", Type = "Savings", Balance = 0, OpenDate = DateOnly.FromDateTime(DateTime.Now), CustomerId = 1, BranchId = 1},
                    new Account {Status = "Active", Type = "Checking", Balance = 0, OpenDate = DateOnly.FromDateTime(DateTime.Now), CustomerId = 2, BranchId = 2},
                    new Account {Status = "Active", Type = "Savings", Balance = 0, OpenDate = DateOnly.FromDateTime(DateTime.Now), CustomerId = 3, BranchId = 3},
                };

            }
        }
    }
}