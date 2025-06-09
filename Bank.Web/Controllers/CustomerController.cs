using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Bank.Models;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;

namespace Bank.Web.Controllers
{
    [Route("customers/[action]/{id?}")]
    public class CustomerController : Controller
    {
        private readonly ApplicationContext context;
        public CustomerController(ApplicationContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var Customers = context.Customers.ToList();  
            return View(Customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer Customer)
        {
            if (context.Customers.Any(b => b.Email.ToLower() == Customer.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Customer Email must be unique.");
            }

            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Customers.Add(Customer);
                context.SaveChanges();
                TempData["success"] = "Customer created successfully!";
                return RedirectToAction("Index");
            }

            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer? Customer = context.Customers.FirstOrDefault(x => x.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }

            return View(Customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer Customer)
        {
            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Customers.Update(Customer);
                context.SaveChanges();
                TempData["success"] = "Customer updated successfully!";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer? customer = context.Customers
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.Transactions)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.FromTransactions)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.ToTransactions)
                .FirstOrDefault(x => x.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            // Delete all transactions associated with the customer's accounts
            foreach (var account in customer.Accounts)
            {
                if (account.Transactions != null)
                {
                    context.Transactions.RemoveRange(account.Transactions);
                }
                if (account.FromTransactions != null)
                {
                    context.Transactions.RemoveRange(account.FromTransactions);
                }
                if (account.ToTransactions != null)
                {
                    context.Transactions.RemoveRange(account.ToTransactions);
                }
            }

            // Delete all accounts
            context.Accounts.RemoveRange(customer.Accounts);

            // Delete the customer
            context.Customers.Remove(customer);
            context.SaveChanges();
            TempData["success"] = "Customer has been deleted successfully!";

            return RedirectToAction("Index");
        }

    }

}
