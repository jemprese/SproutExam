using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Contracts;
using Sprout.Exam.WebApp.Models.Employees;
using Sprout.Exam.WebApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeRepository employeeRepository, ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<EmployeeDto> employeeList = _employeeRepository.GetAllEmployees();

                if (employeeList == null)
                {
                    _logger.LogInformation("No employee list was fetched");
                    return NotFound("Employee list is empty");
                }

                _logger.LogInformation("Successfully retrieved list of employees");
                return Ok(employeeList);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving list of employees");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                EmployeeDto employee = _employeeRepository.GetEmployeeById(id);

                if (employee == null)
                {
                    _logger.LogInformation("No such employee with Id - {EmployeeId}",id);
                    return NotFound("Employee not found");
                }

                _logger.LogInformation("Successfully retrieved information of employee - {EmployeeId}",id);
                return Ok(employee);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employee with Id - {EmployeeId}",id);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            try
            {
                int employeeId = _employeeRepository.UpdateEmployee(input);

                if (employeeId == 0)
                {
                    _logger.LogInformation("No such employee with Id - {EmployeeId}", input.Id);
                    return BadRequest($"Unable to find employee with Id - {input.Id}");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError(ModelState.ToString());
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Employee - {EmployeeId} information successfully updated", input.Id);
                return Ok(employeeId);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating employee - {EmployeeId}", input.Id);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEmployeeDto input)
        {
            try
            {
                if (input == null) return BadRequest("Employee data is null");

                if (!ModelState.IsValid)
                {
                    _logger.LogError(ModelState.ToString());
                    return BadRequest(ModelState);
                }

                int newEmployeeId = _employeeRepository.AddEmployee(input);

                _logger.LogInformation("New employee added - {EmployeeId}", newEmployeeId);

                return Created($"/api/employees/{newEmployeeId}", newEmployeeId);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while adding new employee.");
                return StatusCode(500, new { error = ex.Message });
            }
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int employeeId = _employeeRepository.DeleteEmployee(id);

                if (employeeId == 0)
                {
                    _logger.LogInformation("No such employee with Id - {EmployeeId}", id);
                    return BadRequest($"Unable to delete employee with Id - {id}");
                }

                _logger.LogInformation("Employee - {EmployeeId} information successfully deleted", id);
                return Ok(employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting employee - {EmployeeId}", id);
                return StatusCode(500, new { error = ex.Message });
            }
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate([FromBody] CalculateSalaryDto calculateSalaryDto)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeById(calculateSalaryDto.Id);

                if (employee == null) return NotFound();

                IEmployee employeeFactory = EmployeeFactory.CreateEmployee((EmployeeType)employee.EmployeeTypeId);

                decimal salary = employeeFactory.CalculateSalary(calculateSalaryDto.absentDays, calculateSalaryDto.workedDays);

                _logger.LogInformation("Calculated Salary - {salary}", salary);

                return Ok(salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while computing salary");
                return StatusCode(500, new { error = ex.Message });
            }
        }


    }
}
