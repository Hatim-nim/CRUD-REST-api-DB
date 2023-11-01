using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CRUDoperations.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) 
        {
            
        }

        public DbSet<Employee> Employees { get; set;}
    }
}
