using Sprout.Exam.WebApp.Contracts;
using System;

namespace Sprout.Exam.WebApp.Models.Employees
{
    public class RegularEmployee : IEmployee
    {
        public decimal CalculateSalary(decimal absentDays, decimal workedDays)
        {
            const decimal monthlySalary = 20000;
            const decimal taxDeduction = monthlySalary * 0.12m;
            const int totalWorkingDays = 23;

            decimal netSalary = Math.Round(monthlySalary - (monthlySalary / (totalWorkingDays - absentDays)) - taxDeduction,2);

            return netSalary;
        }
    }
}
