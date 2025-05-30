using CarServiceMVC.Data;
using CarServiceMVC.Models;
using CarServiceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarServiceMVC.Controllers
{
    public class MechanicController : Controller
    {
        private readonly CarServiceDbContext _context;

        public MechanicController(CarServiceDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard(int? customerId, string repairSearchTerm, string repairSearchBy)
        {
            var viewModel = new MechanicDashboardViewModel
            {
                Customers = _context.Customers.ToList()
            };

            if (customerId.HasValue)
            {
                viewModel.SelectedCustomer = _context.Customers
                    .Include(c => c.Cars)
                    .FirstOrDefault(c => c.CustomerID == customerId.Value);

                ViewBag.CustomerID = customerId.Value;
                var repairsQuery = _context.Repairs
                    .Include(r => r.Car)
                    .Where(r => r.Car.CustomerID == customerId.Value);

                if (!string.IsNullOrEmpty(repairSearchTerm) && !string.IsNullOrEmpty(repairSearchBy))
                {
                    if (repairSearchBy == "Model")
                    {
                        repairsQuery = repairsQuery.Where(r => r.Car.Model.Contains(repairSearchTerm));
                    }
                    else if (repairSearchBy == "Date")
                    {
                        if (DateTime.TryParse(repairSearchTerm, out var date))
                        {
                            repairsQuery = repairsQuery.Where(r => r.RepairDate.Date == date.Date);
                        }
                    }
                }

                viewModel.Repairs = repairsQuery.ToList();
            }

            return View(viewModel);
        }

        
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(customer);
        }

        [HttpGet]
        public IActionResult GetRepairInfo(int id)
        {
            var repair = _context.Repairs
                .Include(r => r.Car)
                .Include(r => r.Mechanic)
                .FirstOrDefault(r => r.RepairID == id);

            if (repair == null)
            {
                return NotFound();
            }

            var result = new
            {
                carModel = repair.Car?.Model,
                repairDate = repair.RepairDate.ToString("yyyy-MM-dd"),
                description = repair.Description,
                cost = repair.Cost,
                mechanicName = repair.Mechanic?.Name
            };

            return Json(result);
        }

        [HttpGet]
        public IActionResult CreateRepair(int customerId)
        {
            var customer = _context.Customers
                .Include(c => c.Cars)
                .FirstOrDefault(c => c.CustomerID == customerId);

            if (customer == null)
                return NotFound();

            ViewBag.Cars = new SelectList(customer.Cars, "CarID", "Model");
            ViewBag.CustomerId = customerId;

            return View();
        }




        [HttpPost]
        public IActionResult CreateRepair(int customerId, int CarID, DateTime RepairDate, string Description, decimal Cost)
        {
            var customer = _context.Customers
                .Include(c => c.Cars)
                .FirstOrDefault(c => c.CustomerID == customerId);

            if (customer == null)
                return NotFound();

            var repair = new RepairModel
            {
                CarID = CarID,
                RepairDate = RepairDate,
                Description = Description,
                Cost = Cost
                
            };
            repair.MechanicID = (int)GetLoggedInMechanicId();

            _context.Repairs.Add(repair);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", new { customerId = customerId });
        }



        private int? GetLoggedInMechanicId()
        {

            return HttpContext.Session.GetInt32("MechanicID");
        }

    }
}
