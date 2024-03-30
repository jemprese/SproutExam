
using Sprout.Exam.Business.DataTransferObjects;
using System.Collections.Generic;

namespace Sprout.Exam.WebApp.Contracts
{
    public interface IEmployeeRepository
    {

        #region COMMANDS
        int AddEmployee(CreateEmployeeDto createEmployeeDto);
        int UpdateEmployee(EditEmployeeDto editEmployeeDto);
        int DeleteEmployee(int employeeId);
        #endregion

        #region QUERIES
        EmployeeDto GetEmployeeById(int employeeId);
        IEnumerable<EmployeeDto> GetAllEmployees();
        #endregion
    }
}
