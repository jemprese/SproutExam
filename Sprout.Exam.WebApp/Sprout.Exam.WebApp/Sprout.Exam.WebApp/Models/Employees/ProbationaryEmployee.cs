using Sprout.Exam.WebApp.Contracts;

namespace Sprout.Exam.WebApp.Models.Employees
{
    public class ProbationaryEmployee : IEmployee
    {
        public decimal CalculateSalary(decimal absentDays, decimal workedDays)
        {
            return 0;
        }
    }
}
