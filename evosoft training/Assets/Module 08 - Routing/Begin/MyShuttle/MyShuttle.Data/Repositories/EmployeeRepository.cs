using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyShuttle.Model;

namespace MyShuttle.Data
{
	public class EmployeeRepository : IEmployeeRepository
	{
		MyShuttleContext _context;

		public EmployeeRepository(MyShuttleContext dbcontext)
		{
			_context = dbcontext;
		}

		public async Task<Employee> GetAsync(int employeeId)
		{
			return await _context.Employees
			   .Where(e => e.EmployeeId == employeeId)
			   .SingleOrDefaultAsync();
		}
	}
}