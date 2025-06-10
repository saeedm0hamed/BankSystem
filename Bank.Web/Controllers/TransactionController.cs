using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bank.Data;
using Bank.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Web.Controllers
{
    [Route("transactions/[action]/{id?}")]
    public class TransactionController : Controller
    {
        private readonly ApplicationContext context;

        public TransactionController(ApplicationContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var transactions = context.Transactions
                .Include(t => t.Account)
                .ThenInclude(c => c.Customer)
                .Include(t => t.FromAccount)
                .ThenInclude(c => c.Customer)
                .Include(t => t.ToAccount)
                .ThenInclude(c => c.Customer)
                .OrderByDescending(t => t.Date)
                .ToList();
            return View(transactions);
        }

        public IActionResult Create()
        {
            var accounts = context.Accounts.Where(a => a.Status == "Active").ToList();

            ViewBag.Accounts = new SelectList(accounts.Select(a => new {
                Id = a.Id,
                DisplayText = $"{a.Id} - {a.Balance}$"
            }), "Id", "DisplayText");

            ViewBag.FromAccounts = new SelectList(accounts.Select(a => new {
                Id = a.Id,
                DisplayText = $"{a.Id} - {a.Balance}$"
            }), "Id", "DisplayText");

            ViewBag.ToAccounts = new SelectList(accounts.Select(a => new {
                Id = a.Id,
                DisplayText = $"{a.Id} - {a.Balance}$"
            }), "Id", "DisplayText");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.Date = DateTime.Now;

                if (transaction.Type == "Transfer")
                {
                    var fromAccount = context.Accounts.Find(transaction.FromAccountId);
                    var toAccount = context.Accounts.Find(transaction.ToAccountId);

                    if (fromAccount == null || toAccount == null)
                    {
                        ModelState.AddModelError("", "Invalid account selection for transfer");
                        var accounts = context.Accounts.Where(a => a.Status == "Active").ToList();
                        ViewBag.Accounts = new SelectList(accounts, "Id", "Id");
                        ViewBag.FromAccounts = new SelectList(accounts, "Id", "Id");
                        ViewBag.ToAccounts = new SelectList(accounts, "Id", "Id");
                        return View(transaction);
                    }

                    if (fromAccount.Balance < transaction.Amount)
                    {
                        ModelState.AddModelError("", "Insufficient funds for transfer");
                        var accounts = context.Accounts.Where(a => a.Status == "Active").ToList();
                        ViewBag.Accounts = new SelectList(accounts, "Id", "Id");
                        ViewBag.FromAccounts = new SelectList(accounts, "Id", "Id");
                        ViewBag.ToAccounts = new SelectList(accounts, "Id", "Id");
                        return View(transaction);
                    }

                    fromAccount.Balance -= transaction.Amount;
                    toAccount.Balance += transaction.Amount;

                    var transferTransaction = new Transaction
                    {
                        Date = transaction.Date,
                        Amount = transaction.Amount,
                        Type = "Transfer",
                        FromAccountId = transaction.FromAccountId,
                        ToAccountId = transaction.ToAccountId,
                        FromAccount = fromAccount,
                        ToAccount = toAccount
                    };

                    context.Transactions.Add(transferTransaction);
                }
                else
                {
                    var account = context.Accounts.Find(transaction.AccountId);
                    if (account == null)
                    {
                        ModelState.AddModelError("", "Invalid account selection");
                        var accounts = context.Accounts.Where(a => a.Status == "Active").ToList();
                        ViewBag.Accounts = new SelectList(accounts, "Id", "Id");
                        ViewBag.FromAccounts = new SelectList(accounts, "Id", "Id");
                        ViewBag.ToAccounts = new SelectList(accounts, "Id", "Id");
                        return View(transaction);
                    }

                    if (transaction.Type == "Deposit")
                    {
                        account.Balance += transaction.Amount;
                    }
                    else if (transaction.Type == "Withdrawal")
                    {
                        if (account.Balance < transaction.Amount)
                        {
                            ModelState.AddModelError("", "Insufficient funds for withdrawal");
                            var accounts = context.Accounts.Where(a => a.Status == "Active").ToList();
                            ViewBag.Accounts = new SelectList(accounts, "Id", "Id");
                            ViewBag.FromAccounts = new SelectList(accounts, "Id", "Id");
                            ViewBag.ToAccounts = new SelectList(accounts, "Id", "Id");
                            return View(transaction);
                        }
                        account.Balance -= transaction.Amount;
                    }

                    context.Transactions.Add(transaction);
                }

                context.SaveChanges();
                TempData["success"] = "Transaction created successfully!";
                return RedirectToAction("Index");
            }

            var accountsList = context.Accounts.Where(a => a.Status == "Active").ToList();
            ViewBag.Accounts = new SelectList(accountsList, "Id", "Id");
            ViewBag.FromAccounts = new SelectList(accountsList, "Id", "Id");
            ViewBag.ToAccounts = new SelectList(accountsList, "Id", "Id");
            return View(transaction);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transaction? transaction = context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .FirstOrDefault(x => x.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            context.Transactions.Remove(transaction);
            context.SaveChanges();
            TempData["success"] = "Transaction deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}