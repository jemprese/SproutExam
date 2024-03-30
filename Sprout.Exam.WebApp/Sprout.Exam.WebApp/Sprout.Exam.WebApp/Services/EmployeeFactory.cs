using Microsoft.AspNetCore.Http;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Contracts;
using Sprout.Exam.WebApp.Models.Employees;
using System;

namespace Sprout.Exam.WebApp.Services
{
    public class EmployeeFactory
    {
        public static IEmployee CreateEmployee(EmployeeType employeeType)
        {
            switch(employeeType)
            {
                case EmployeeType.Regular:
                    return new RegularEmployee();
                case EmployeeType.Contractual: 
                    return new ContractualEmployee();
                case EmployeeType.Probationary:
                    return new ProbationaryEmployee();
                case EmployeeType.PartTime:
                    return new PartTimeEmployee();
                default:
                    throw new NotImplementedException("Salary calculation is not yet implemented for this employee type.");
            }
        }
    }
}
