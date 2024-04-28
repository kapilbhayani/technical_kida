using DataAccessLeyer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using technology_kida.Repository;

namespace technology_kida.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class employeeAPI : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public employeeAPI(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<employee>> getAllEmp()
        {

            try
            {
                return Ok(await _employeeRepository.GetAllEmployees());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retriving data from database");
            }

        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<employee>> getEmpByID(int id)
        {
            try
            {
                var matchdata = await _employeeRepository.GetEmployee(id);
                if (matchdata == null)
                {
                    return NotFound();
                }

                return Ok(matchdata);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retriving data from database");

            }
        }

        [HttpPost]
        public async Task<ActionResult<employee>> createEmployee([FromBody] employee newEmp)
        {
            try
            {
                List<employee> allEmp = await _employeeRepository.GetAllEmployees();
                var matchEmp = allEmp.FirstOrDefault(data => data.Name == newEmp.Name || data.City == newEmp.City);

                if (newEmp == null)
                {
                    return BadRequest();
                }
                else if (matchEmp != null)
                {
                    ModelState.AddModelError("Coustom Error", "data is already exit Please differnt data insert");
                    return BadRequest(ModelState);
                }
                var employee = await _employeeRepository.AddEmployee(newEmp);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult<employee>> UpdateEmployee(int id, [FromBody] employee updateEmp)
        {
            try
            {
                if (id != updateEmp.Id)
                {
                    return BadRequest("id mismatch");
                }

                var foundData = await _employeeRepository.GetEmployee(id);

                if (foundData == null)
                {
                    return NotFound($"data is not found for this Id ={id}");
                }

                return Ok(await _employeeRepository.UpdateEmployee(updateEmp));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult<employee>> deleteEmployee(int id)
        {
            try
            {
                var foundData = await _employeeRepository.GetEmployee(id);
                if (foundData == null)
                {
                    return BadRequest();
                }

                var deleteEmp = await _employeeRepository.DeleteEmployee(id);
                return Ok(deleteEmp);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }


        [HttpGet("{search:alpha}")]

        public async Task<ActionResult<IEnumerable<employee>>> searchEmp(string name)
        {
            try
            {
                var result = await _employeeRepository.Search(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

    }
}
