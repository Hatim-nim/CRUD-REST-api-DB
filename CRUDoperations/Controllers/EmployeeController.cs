using CRUDoperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDoperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _dbContext;

        public EmployeeController(EmployeeContext dbContext)
        {
            _dbContext = dbContext;

        }
        // Read
        [HttpGet] // Get method here
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }
            return await _dbContext.Employees.ToListAsync();
        }

        //passing id // Read
        [HttpGet("{id}")] // Get method here
        public async Task<ActionResult<Employee>> GetEmployees(int id)
        {
            if (_dbContext.Employees == null)
            {
                return NotFound(); // if theres no emp, return notfound
            }
            // we can use await bcs its async method
            var employee = await _dbContext.Employees.FindAsync(id); // if found, return
            if (employee == null)
            {
                return NotFound();
            }

            return employee; // return particular emp
        }

        // Create
        [HttpPost] // passed from front-end
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            // it'll go to the dbContext and implement Add method
            _dbContext.Employees.Add(employee); // this emp is gotten from the front-end
            await _dbContext.SaveChangesAsync();
            // return same emp
            return CreatedAtAction(nameof(GetEmployees), new { id = employee.EmployeeId }, employee);
        }

        [HttpPut] // passing id of the emp that we want to update
        public async Task<ActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId) {
                return BadRequest();
            }
            _dbContext.Entry(employee).State = EntityState.Modified; // we are going to update our DB

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(isEmpAvailable(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw; // throw same db exception
                }
            }
            return Ok(); // if it gets here then this means mission successful
        }

        // whatever id we're passing, this method will tell us if its available or not
        private bool isEmpAvailable(int id)
        { return (_dbContext.Employees?.Any(x => x.EmployeeId == id)).GetValueOrDefault();
        }


        [HttpDelete("{id}")] // delete
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if (_dbContext.Employees== null)
            {
                return NotFound();
            }

            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            // if we got here, we can delete successfully
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }

}

