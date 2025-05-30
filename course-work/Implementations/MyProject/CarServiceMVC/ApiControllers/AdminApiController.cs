using CarServiceMVC.Data;
using CarServiceMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarServiceMVC.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly CarServiceDbContext _context;

        public AdminApiController(CarServiceDbContext context)
        {
            _context = context;
        }

        // ==== CUSTOMERS ====

        [HttpGet("customers")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        [HttpPost("customers")]
        public async Task<ActionResult<CustomerModel>> CreateCustomer(CustomerModel customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomers), new { id = customer.CustomerID }, customer);
        }

        [HttpPut("customers/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerModel customer)
        {
            if (id != customer.CustomerID)
                return BadRequest();

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("customers/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Cars)
                .FirstOrDefaultAsync(c => c.CustomerID == id);

            if (customer == null)
                return NotFound();

            if (customer.Cars.Any())
                return BadRequest("Cannot delete customer with cars.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ==== MECHANICS ====

        [HttpGet("mechanics")]
        public async Task<ActionResult<IEnumerable<MechanicModel>>> GetMechanics()
        {
            return await _context.Mechanics.ToListAsync();
        }

        [HttpPost("mechanics")]
        public async Task<ActionResult<MechanicModel>> CreateMechanic(MechanicModel mechanic)
        {
            _context.Mechanics.Add(mechanic);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMechanics), new { id = mechanic.MechanicID }, mechanic);
        }

        [HttpPut("mechanics/{id}")]
        public async Task<IActionResult> UpdateMechanic(int id, MechanicModel mechanic)
        {
            if (id != mechanic.MechanicID)
                return BadRequest();

            _context.Entry(mechanic).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("mechanics/{id}")]
        public async Task<IActionResult> DeleteMechanic(int id)
        {
            var mechanic = await _context.Mechanics.FindAsync(id);
            if (mechanic == null)
                return NotFound();

            _context.Mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ==== CARS ====

        [HttpGet("cars")]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetCars()
        {
            return await _context.Cars.Include(c => c.Customer).ToListAsync();
        }

        [HttpPost("cars")]
        public async Task<ActionResult<CarModel>> CreateCar(CarModel car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCars), new { id = car.CarID }, car);
        }

        [HttpPut("cars/{id}")]
        public async Task<IActionResult> UpdateCar(int id, CarModel car)
        {
            if (id != car.CarID)
                return BadRequest();

            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("cars/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Repairs)
                .FirstOrDefaultAsync(c => c.CarID == id);

            if (car == null)
                return NotFound();

            if (car.Repairs.Any())
                return BadRequest("Cannot delete car with repairs.");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ==== REPAIRS ====

        [HttpGet("repairs")]
        public async Task<ActionResult<IEnumerable<RepairModel>>> GetRepairs()
        {
            return await _context.Repairs
                .Include(r => r.Car)
                .Include(r => r.Mechanic)
                .ToListAsync();
        }

        [HttpPost("repairs")]
        public async Task<ActionResult<RepairModel>> CreateRepair(RepairModel repair)
        {
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRepairs), new { id = repair.RepairID }, repair);
        }

        [HttpPut("repairs/{id}")]
        public async Task<IActionResult> UpdateRepair(int id, RepairModel repair)
        {
            if (id != repair.RepairID)
                return BadRequest();

            _context.Entry(repair).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("repairs/{id}")]
        public async Task<IActionResult> DeleteRepair(int id)
        {
            var repair = await _context.Repairs.FindAsync(id);
            if (repair == null)
                return NotFound();

            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
