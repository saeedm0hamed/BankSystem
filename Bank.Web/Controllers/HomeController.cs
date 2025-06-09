using System.Diagnostics;
using Bank.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Schema()
        {
            TempData["schema"] = true;
            return RedirectToAction("Index");
        }
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
