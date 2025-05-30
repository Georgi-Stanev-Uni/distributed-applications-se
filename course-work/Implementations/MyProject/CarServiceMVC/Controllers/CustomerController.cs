using CarServiceMVC.Data;
using CarServiceMVC.Models;
using CarServiceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.ConstrainedExecution;

namespace CarServiceMVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CarServiceDbContext _context;

        public CustomerController(CarServiceDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Dashboard(int? carId = null)
        {
            var customerId = GetLoggedInCustomerId();

            var cars = await _context.Cars
                .Where(c => c.CustomerID == customerId)
                .ToListAsync();

            if (!cars.Any())
                return View(new CustomerDashboardViewModel { Cars = new List<CarModel>(), SelectedCar = null });

            
            var selectedCarId = carId ?? cars.First().CarID;

            var selectedCar = await _context.Cars
                .Include(c => c.Repairs)
                .ThenInclude(r => r.Mechanic)
                .FirstOrDefaultAsync(c => c.CarID == selectedCarId && c.CustomerID == customerId);

            var viewModel = new CustomerDashboardViewModel
            {
                Cars = cars,
                SelectedCar = selectedCar
            };

            return View(viewModel);
        }

        
        public IActionResult CreateCar()
        {
            return View();
            
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateCar(CarModel car, IFormFile ImageFile)
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
                return RedirectToAction("Login", "Home");

            car.CustomerID = customerId.Value;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cars");
                Directory.CreateDirectory(uploadsFolder); 

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                car.ImagePath = Path.Combine("images", "cars", uniqueFileName); 
            }

            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }

            return View(car);
        }


        [HttpGet]
        public IActionResult GetCarInfo(int id)
        {
            var car = _context.Cars
                .Where(c => c.CarID == id)
                .Select(c => new
                {
                    carID = c.CarID,
                    make = c.Make,
                    model = c.Model,
                    year = c.Year,
                    licensePlate = c.LicensePlate
                })
                .FirstOrDefault();

            if (car == null)
                return NotFound();

            return Json(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCar(CarModel updatedCar)
        {
            if (!ModelState.IsValid)
            {
                
                return BadRequest(ModelState);
            }

            var car = _context.Cars.Find(updatedCar.CarID);
            if (car == null)
            {
                return NotFound();
            }

            
            car.Make = updatedCar.Make;
            car.Model = updatedCar.Model;
            car.Year = updatedCar.Year;
            car.LicensePlate = updatedCar.LicensePlate;

            _context.SaveChanges();

            
            return RedirectToAction("Dashboard", new { carId = car.CarID });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCar(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null)
                return NotFound();

            _context.Cars.Remove(car);
            _context.SaveChanges();
            return Ok();
        }





        private int? GetLoggedInCustomerId()
        {

            return HttpContext.Session.GetInt32("CustomerID");
        }
    }

}
