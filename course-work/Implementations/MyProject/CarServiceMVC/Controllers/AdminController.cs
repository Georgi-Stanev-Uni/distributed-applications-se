using CarServiceMVC.Data;
using CarServiceMVC.Models;
using CarServiceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarServiceMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly CarServiceDbContext _context;

        public AdminController(CarServiceDbContext context)
        {
            _context = context;
        }

        public IActionResult AdminPanel()
        {
            var viewModel = new AdminPanelViewModel
            {
                Customers = _context.Customers.ToList(),
                Mechanics = _context.Mechanics.ToList(),
                Cars = _context.Cars.ToList(),
                Repairs = _context.Repairs.ToList()
            };

            return View(viewModel);
        }
        //customer
        #region
        
        public IActionResult CreateCustomer()
        {
            return View("Customer/CreateCustomer");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCustomer(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("AdminPanel");
            }

            return View( customer);
        }

        
        [HttpGet]
        public IActionResult DeleteCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCustomerConfirmed(int id)
        {
            var customer = _context.Customers
                .Include(c => c.Cars) 
                .FirstOrDefault(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }

            if (customer.Cars != null)
            {
                _context.Cars.RemoveRange(customer.Cars);
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
                return NotFound();

            return View("Customer/UpdateCustomer", customer); // path: Views/Admin/Customers/EditCustomer.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCustomer(CustomerModel updatedCustomer)
        {
            if (!ModelState.IsValid)
                return View("Customer/UpdateCustomer", updatedCustomer);

            var existingCustomer = _context.Customers.FirstOrDefault(c => c.CustomerID == updatedCustomer.CustomerID);
            if (existingCustomer == null)
                return NotFound();

            existingCustomer.Name = updatedCustomer.Name;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.PasswordHash = updatedCustomer.PasswordHash;
            existingCustomer.Phone = updatedCustomer.Phone;
            existingCustomer.Address = updatedCustomer.Address;

            _context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        public IActionResult DeleteMechanic(int id)
        {
            var mechanic = _context.Mechanics
                .Include(m => m.Repairs)
                .FirstOrDefault(m => m.MechanicID == id);

            if (mechanic == null)
                return NotFound();

            if (mechanic.Repairs != null && mechanic.Repairs.Any())
            {
                TempData["Error"] = "Cannot delete mechanic with associated repairs.";
                return RedirectToAction("AdminPanel");
            }

            _context.Mechanics.Remove(mechanic);
            _context.SaveChanges();

            TempData["Success"] = "Mechanic deleted successfully.";
            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        public IActionResult UpdateMechanic(int id)
        {
            var mechanic = _context.Mechanics.FirstOrDefault(m => m.MechanicID == id);
            if (mechanic == null)
                return NotFound();

            return View("Mechanic/UpdateMechanic", mechanic);
        }

        [HttpPost]
        public IActionResult UpdateMechanic(MechanicModel updatedMechanic)
        {
            if (!ModelState.IsValid)
                return View("Mechanic/UpdateMechanic", updatedMechanic);

            var existing = _context.Mechanics.FirstOrDefault(m => m.MechanicID == updatedMechanic.MechanicID);
            if (existing == null)
                return NotFound();

            existing.Name = updatedMechanic.Name;
            existing.Email = updatedMechanic.Email;
            existing.PasswordHash = updatedMechanic.PasswordHash;
            existing.Address = updatedMechanic.Address;
            existing.Phone = updatedMechanic.Phone;
            existing.HireDate = updatedMechanic.HireDate;
            existing.HourlyRate = updatedMechanic.HourlyRate;
            existing.Specialization = updatedMechanic.Specialization;
            existing.YearsOfExperience = updatedMechanic.YearsOfExperience;

            _context.SaveChanges();

            TempData["Success"] = "Mechanic updated successfully.";
            return RedirectToAction("AdminPanel");
        }




        #endregion
        //mechanic
        #region
        [HttpGet]
        public IActionResult CreateMechanic()
        {
            return View("Mechanic/CreateMechanic");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMechanic(MechanicModel mechanic)
        {
            if (ModelState.IsValid)
            {
                _context.Mechanics.Add(mechanic);
                _context.SaveChanges();
                return RedirectToAction("AdminPanel");
            }

            return View(mechanic);
        }
        #endregion
        //car
        #region
        [HttpGet]
        public IActionResult CreateCar()
        {
            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "CustomerID", "Name");
            return View("Car/CreateCar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCar(CarModel car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                _context.SaveChanges();
                return RedirectToAction("AdminPanel");
            }

            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "CustomerID", "Name", car.CustomerID);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCar(int id)
        {
            var car = _context.Cars
                .Include(c => c.Repairs)
                .FirstOrDefault(c => c.CarID == id);

            if (car == null)
                return NotFound();

            if (car.Repairs != null && car.Repairs.Any())
            {
                TempData["Error"] = "Cannot delete car with existing repairs.";
                return RedirectToAction("AdminPanel");
            }

            _context.Cars.Remove(car);
            _context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        public IActionResult UpdateCar(int id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.CarID == id);
            if (car == null)
                return NotFound();

            ViewBag.Customers = new SelectList(_context.Customers.ToList(), "CustomerID", "Name", car.CustomerID);
            return View("Car/UpdateCar", car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCar(CarModel updatedCar)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = new SelectList(_context.Customers.ToList(), "CustomerID", "Name", updatedCar.CustomerID);
                return View(updatedCar);
            }

            var existingCar = _context.Cars.FirstOrDefault(c => c.CarID == updatedCar.CarID);
            if (existingCar == null)
                return NotFound();

            existingCar.CustomerID = updatedCar.CustomerID;
            existingCar.Make = updatedCar.Make;
            existingCar.Model = updatedCar.Model;
            existingCar.Year = updatedCar.Year;
            existingCar.LicensePlate = updatedCar.LicensePlate;
            existingCar.ImagePath = updatedCar.ImagePath;

            _context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }


        #endregion
        //repair
        #region
        [HttpGet]
        public IActionResult CreateRepair()
        {
            // Load all cars with ID, Make, and Model for dropdown
            var cars = _context.Cars
                .Select(c => new
                {
                    c.CarID,
                    DisplayName = c.CarID + " - " + c.Make + " " + c.Model
                })
                .ToList();

            ViewBag.Cars = new SelectList(cars, "CarID", "DisplayName");

            // Load all mechanics for dropdown
            var mechanics = _context.Mechanics.ToList();
            ViewBag.Mechanics = new SelectList(mechanics, "MechanicID", "Name");

            return View("Repair/CreateRepair");
        }



        [HttpPost]
        public IActionResult CreateRepair(int carId, int mechanicId, DateTime repairDate, string description, decimal cost)
        {
            // Validate car exists
            var car = _context.Cars.FirstOrDefault(c => c.CarID == carId);
            if (car == null)
                return NotFound("Car not found.");

            // Validate mechanic exists
            var mechanic = _context.Mechanics.FirstOrDefault(m => m.MechanicID == mechanicId);
            if (mechanic == null)
                return NotFound("Mechanic not found.");

            var repair = new RepairModel
            {
                CarID = carId,
                MechanicID = mechanicId,
                RepairDate = repairDate,
                Description = description,
                Cost = cost
            };

            _context.Repairs.Add(repair);
            _context.SaveChanges();

            // Redirect back to AdminPanel or wherever appropriate
            return RedirectToAction("AdminPanel");
        }


        private void LoadDropdowns(RepairModel repair)
        {
            var cars = _context.Cars.ToList();
            ViewBag.Cars = new SelectList(cars.Select(c => new { c.CarID, Display = $"{c.Make} {c.Model} (ID: {c.CarID})" }), "CarID", "Display", repair.CarID);

            var mechanics = _context.Mechanics.ToList();
            ViewBag.Mechanics = new SelectList(mechanics, "MechanicID", "Name", repair.MechanicID);
        }

        [HttpPost]
        public IActionResult DeleteRepair(int id)
        {
            var repair = _context.Repairs.FirstOrDefault(r => r.RepairID == id);

            if (repair == null)
            {
                return NotFound();
            }

            _context.Repairs.Remove(repair);
            _context.SaveChanges();

            return RedirectToAction("AdminPanel"); // or any relevant view
        }

        [HttpGet]
        public IActionResult UpdateRepair(int id)
        {
            var repair = _context.Repairs
        .Include(r => r.Car)
        .Include(r => r.Mechanic)
        .FirstOrDefault(r => r.RepairID == id);

            if (repair == null)
                return NotFound();

            // Car dropdown with custom display text: "Model (ID: X)"
            var cars = _context.Cars
                .Select(c => new
                {
                    CarID = c.CarID,
                    DisplayText = c.Model + " (ID: " + c.CarID + ")"
                }).ToList();

            ViewBag.Cars = new SelectList(cars, "CarID", "DisplayText", repair.CarID);
            ViewBag.Mechanics = new SelectList(_context.Mechanics.ToList(), "MechanicID", "Name", repair.MechanicID);

            return View("Repair/UpdateRepair", repair); // Views/Admin/Repairs/EditRepair.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRepair(RepairModel updatedRepair)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cars = new SelectList(_context.Cars
                .Select(c => new
                {
                    CarID = c.CarID,
                    DisplayText = c.Model + " (ID: " + c.CarID + ")"
                }).ToList(), "CarID", "DisplayText", updatedRepair.CarID);

                ViewBag.Mechanics = new SelectList(_context.Mechanics.ToList(), "MechanicID", "Name", updatedRepair.MechanicID);
                return View("Repair/UpdateRepair", updatedRepair);
            }

            var existingRepair = _context.Repairs.FirstOrDefault(r => r.RepairID == updatedRepair.RepairID);
            if (existingRepair == null)
                return NotFound();

            existingRepair.CarID = updatedRepair.CarID;
            existingRepair.MechanicID = updatedRepair.MechanicID;
            existingRepair.RepairDate = updatedRepair.RepairDate;
            existingRepair.Description = updatedRepair.Description;
            existingRepair.Cost = updatedRepair.Cost;

            _context.SaveChanges();

            return RedirectToAction("AdminPanel");
        }


        #endregion

    }



}
