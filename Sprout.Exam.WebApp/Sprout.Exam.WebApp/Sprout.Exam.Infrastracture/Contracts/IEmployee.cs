namespace Sprout.Exam.WebApp.Contracts
{
    public interface IEmployee
    {
        decimal CalculateSalary(decimal absentDays,decimal workedDays);
    }
}
