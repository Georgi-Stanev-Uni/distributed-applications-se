using Microsoft.AspNetCore.Mvc;
using CarServiceMVC.Models;
using CarServiceMVC.Data;
using System.Linq;

namespace CarServiceMVC.Controllers
{
    public class LogInController : Controller
    {
        private readonly CarServiceDbContext _context;

        public LogInController(CarServiceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LogInViewModel());
        }

        [HttpPost]
        public IActionResult Index(LogInViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (model.Name == "admin" && model.Password == "admin")
            {
                return RedirectToAction("AdminPanel","Admin");
            }
            if (model.Role == "Customer")
            {
                var customer = _context.Customers
                    .FirstOrDefault(c => c.Name == model.Name && c.PasswordHash == model.Password); 

                if (customer != null)
                {
                    HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                    return RedirectToAction("Dashboard", "Customer");
                }                    
            }
            else if (model.Role == "Mechanic")
            {
                var mechanic = _context.Mechanics
                    .FirstOrDefault(m => m.Name == model.Name && m.PasswordHash == model.Password);

                if (mechanic != null)
                {
                    HttpContext.Session.SetInt32("MechanicID", mechanic.MechanicID);
                    return RedirectToAction("Dashboard", "Mechanic");
                }

            }

            ModelState.AddModelError(string.Empty, "Invalid login credentials.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  
            return RedirectToAction("Index", "LogIn");  
        }
    }
}
