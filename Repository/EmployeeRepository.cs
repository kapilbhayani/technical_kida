using DataAccessLeyer;
using Microsoft.EntityFrameworkCore;
using technology_kida.data;

namespace technology_kida.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly applicationDbContext _db;
        public EmployeeRepository(applicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<employee>> GetAllEmployees()
        {
            return await _db.employees.ToListAsync();
        }

        public async Task<employee> GetEmployee(int employeeId)
        {
            return await _db.employees.FirstOrDefaultAsync(emp => emp.Id == employeeId);
        }

        public async Task<employee> AddEmployee(employee emp)
        {
            var result = await _db.employees.AddAsync(emp);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<employee> DeleteEmployee(int employeeId)
        {
            var result = await _db.employees.FirstOrDefaultAsync(data => data.Id == employeeId);
            if (result != null)
            {
                _db.employees.Remove(result);
                await _db.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<employee> UpdateEmployee(employee emp)
        {
            var matchData = await _db.employees.FirstOrDefaultAsync(data => data.Id == emp.Id);

            if (matchData != null)
            {
                matchData.Name = emp.Name;
                matchData.City = emp.City;
                await _db.SaveChangesAsync();
                return matchData;
            }
            return null;
        }

        public async Task<IEnumerable<employee>> Search(string name)
        {
            IQueryable<employee> query = _db.employees; // Initialize as IQueryable<Employee>

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name)); // Apply Where clause
            }

            return await query.ToListAsync(); // Execute query asynchronously
        }

    }
}

