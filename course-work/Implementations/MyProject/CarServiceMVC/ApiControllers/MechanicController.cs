using CarServiceMVC.Data;
using CarServiceMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarServiceMVC.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MechanicApiController : ControllerBase
    {
        private readonly CarServiceDbContext _context;

        public MechanicApiController(CarServiceDbContext context)
        {
            _context = context;
        }

        // ==== GET ALL REPAIRS DONE BY MECHANIC ====

        [HttpGet("repairs/{mechanicId}")]
        public async Task<ActionResult<IEnumerable<RepairModel>>> GetRepairsByMechanic(int mechanicId)
        {
            var repairs = await _context.Repairs
                .Include(r => r.Car)
                .Include(r => r.Mechanic)
                .Where(r => r.MechanicID == mechanicId)
                .ToListAsync();

            return repairs;
        }

        // ==== CREATE REPAIR ====

        [HttpPost("repairs")]
        public async Task<ActionResult<RepairModel>> CreateRepair([FromBody] RepairModel repair)
        {
            if (!_context.Mechanics.Any(m => m.MechanicID == repair.MechanicID))
                return BadRequest("Invalid Mechanic ID");

            if (!_context.Cars.Any(c => c.CarID == repair.CarID))
                return BadRequest("Invalid Car ID");

            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRepairById), new { id = repair.RepairID }, repair);
        }

        // ==== GET SINGLE REPAIR ====

        [HttpGet("repair/{id}")]
        public async Task<ActionResult<RepairModel>> GetRepairById(int id)
        {
            var repair = await _context.Repairs
                .Include(r => r.Car)
                .Include(r => r.Mechanic)
                .FirstOrDefaultAsync(r => r.RepairID == id);

            if (repair == null)
                return NotFound();

            return repair;
        }

        // ==== UPDATE REPAIR ====

        [HttpPut("repairs/{id}")]
        public async Task<IActionResult> UpdateRepair(int id, [FromBody] RepairModel updatedRepair)
        {
            if (id != updatedRepair.RepairID)
                return BadRequest();

            _context.Entry(updatedRepair).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==== DELETE REPAIR ====

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

        // ==== GET CUSTOMERS THAT MECHANIC HAS WORKED WITH ====

        [HttpGet("customers/{mechanicId}")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomersByMechanic(int mechanicId)
        {
            var customers = await _context.Repairs
                .Where(r => r.MechanicID == mechanicId)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Customer)
                .Select(r => r.Car.Customer)
                .Distinct()
                .ToListAsync();

            return customers;
        }

        // ==== GET CARS BY CUSTOMER ID ====

        [HttpGet("cars/{customerId}")]
        public async Task<ActionResult<IEnumerable<CarModel>>> GetCarsByCustomer(int customerId)
        {
            return await _context.Cars
                .Where(c => c.CustomerID == customerId)
                .ToListAsync();
        }
    }
}
