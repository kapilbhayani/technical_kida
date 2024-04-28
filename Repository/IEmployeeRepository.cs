using DataAccessLeyer;

namespace technology_kida.Repository
{
    public interface IEmployeeRepository
    {

        Task<List<employee>> GetAllEmployees();

        Task<employee> GetEmployee(int employeeId);

        Task<employee> AddEmployee(employee emp);

        Task<employee> UpdateEmployee(employee emp);

        Task<employee> DeleteEmployee(int employeeId);

        Task<IEnumerable<employee>> Search(string name);
    }
}
