using Sprout.Exam.WebApp.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Sprout.Exam.WebApp.Models.Employees
{
    public class PartTimeEmployee : IEmployee
    {
        public decimal CalculateSalary(decimal absentDays, decimal workedDays)
        {
            return 0;
        }
    }
}
