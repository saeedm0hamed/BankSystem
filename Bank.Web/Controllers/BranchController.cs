using Microsoft.AspNetCore.Mvc;
using Bank.Data;
using Bank.Models;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Web.Controllers
{
    [Route("branches/[action]/{id?}")]
    public class BranchController : Controller
    {
        private readonly ApplicationContext context;
        public BranchController(ApplicationContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var branches = context.Branches.Include(b => b.Employees).ToList();  
            return View(branches);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Branch branch)
        {
            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Branches.Add(branch);
                context.SaveChanges();
                TempData["success"] = "Branch created successfully!";
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

            Branch? branch = context.Branches.FirstOrDefault(x => x.Id == id);

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        [HttpPost]
        public IActionResult Edit(Branch branch)
        {
            if (ModelState.IsValid) // Valid ~ passed all validations in model
            {
                context.Branches.Update(branch);
                context.SaveChanges();
                TempData["success"] = "Branch updated successfully!";
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

            Branch? branch = context.Branches.FirstOrDefault(x => x.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            context.Branches.Remove(branch);
            context.SaveChanges();
            TempData["success"] = "Branch deleted successfully!";

            return RedirectToAction("Index");
        }

    }

}
