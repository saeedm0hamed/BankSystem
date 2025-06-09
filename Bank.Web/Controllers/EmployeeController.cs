using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Bank.Models;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Web.Controllers
{
    [Route("employees/[action]/{id?}")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext context;
        public EmployeeController(ApplicationContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var Employees = context.Employees.Include(b => b.Branch).ToList();  
            return View(Employees);
        }

        public IActionResult Create()
        {
            ViewBag.Branches = new SelectList(context.Branches, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee Employee)
        {
            if (context.Employees.Any(b => b.Email.ToLower() == Employee.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Employee Email must be unique.");
            }

            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Employees.Add(Employee);
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

            Employee? Employee = context.Employees.FirstOrDefault(x => x.Id == id);

            if (Employee == null)
            {
                return NotFound();
            }

            ViewBag.Branches = new SelectList(context.Branches, "Id", "Name");
            return View(Employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee Employee)
        {
            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Employees.Update(Employee);
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

            Employee? Employee = context.Employees.FirstOrDefault(x => x.Id == id);
            if (Employee == null)
            {
                return NotFound();
            }

            context.Employees.Remove(Employee);
            context.SaveChanges();
            TempData["success"] = "Employee deleted successfully!";

            return RedirectToAction("Index");
        }

    }

}
