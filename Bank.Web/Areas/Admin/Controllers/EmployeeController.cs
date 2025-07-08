using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Bank.Models;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bank.Web.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Bank.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("employees/[action]/{id?}")]
    [Authorize(Roles = Roles.Admin)]
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext context;
        public EmployeeController(ApplicationContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var employees = context.Employees.Include(b => b.Branch).ToList();
            return View(employees);
        }

        public IActionResult Create()
        {
            ViewBag.Branches = new SelectList(context.Branches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Bank.Models.Employee employee)
        {
            if (context.Employees.Any(b => b.Email.ToLower() == employee.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Employee Email must be unique.");
            }

            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Employees.Add(employee);
                context.SaveChanges();
                TempData["success"] = "Employee created successfully!";
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

            Bank.Models.Employee? employee = context.Employees.FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Branches = new SelectList(context.Branches, "Id", "Name");
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Bank.Models.Employee employee)
        {
            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Employees.Update(employee);
                context.SaveChanges();
                TempData["success"] = "Employee updated successfully!";
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

            Bank.Models.Employee? employee = context.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            context.Employees.Remove(employee);
            context.SaveChanges();
            TempData["success"] = "Employee deleted successfully!";

            return RedirectToAction("Index");
        }

    }

}
