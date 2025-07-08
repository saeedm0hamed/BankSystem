using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Bank.Web.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var customers = context.Customers.ToList();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Bank.Models.Customer customer)
        {
            if (context.Customers.Any(b => b.Email.ToLower() == customer.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Customer Email must be unique.");
            }

            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Customers.Add(customer);
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

            Bank.Models.Customer? customer = context.Customers.FirstOrDefault(x => x.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Bank.Models.Customer customer)
        {
            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Customers.Update(customer);
                context.SaveChanges();
                TempData["success"] = "Customer updated successfully!";
                return RedirectToAction("Index");
            }

            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bank.Models.Customer? customer = context.Customers
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
