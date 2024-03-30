using Sprout.Exam.WebApp.Contracts;
using System;

namespace Sprout.Exam.WebApp.Models.Employees
{
    public class ContractualEmployee : IEmployee
    {
        public decimal CalculateSalary(decimal absentDays,decimal workedDays)
        {
            const decimal ratePerDay = 500;

            decimal netSalary = Math.Round(ratePerDay * workedDays,2);

            return netSalary;
        }
    }
}
