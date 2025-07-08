using System.Diagnostics;
using Bank.Models;
using Bank.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/schema")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Schema()
        {
            TempData["schema"] = true;
            return RedirectToAction("Index");
        }
        [Route("/erd")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult ERD()
        {
            TempData["erd"] = true;
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
