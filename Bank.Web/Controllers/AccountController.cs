using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Bank.Models;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Web.Controllers
{
    [Route("accounts/[action]/{id?}")]
    public class AccountController : Controller
    {
        private readonly ApplicationContext context;
        public AccountController(ApplicationContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var accounts = context.Accounts
                .Include(b => b.Customer)
                .Include(c => c.Branch)
                .Include(a => a.Transactions)
                .Include(a => a.FromTransactions)
                .Include(a => a.ToTransactions)
                .ToList();  
            return View(accounts);
        }

        public IActionResult Create()
        {
            var customers = context.Customers.Where(x => !x.Accounts.Any()).ToList();
            ViewBag.Customers = new SelectList(customers, "Id", "Name");
            ViewBag.Branches = new SelectList(context.Branches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Account Account)
        {
            if (ModelState.IsValid)
            {
                context.Accounts.Add(Account);
                context.SaveChanges();
                TempData["success"] = "Account created successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.Customers = new SelectList(context.Customers, "Id", "Name");
            ViewBag.Branches = new SelectList(context.Branches, "Id", "Name");
            return View(Account);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Account? Account = context.Accounts.FirstOrDefault(x => x.Id == id);

            if (Account == null)
            {
                return NotFound();
            }

            return View(Account);
        }

        [HttpPost]
        public IActionResult Edit(Account Account)
        {
            if (ModelState.IsValid)
            {
                context.Accounts.Update(Account);
                context.SaveChanges();
                TempData["success"] = "Account updated successfully!";
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

            Account? Account = context.Accounts.FirstOrDefault(x => x.Id == id);
            if (Account == null)
            {
                return NotFound();
            }

            context.Accounts.Remove(Account);
            context.SaveChanges();
            TempData["success"] = "Account deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}