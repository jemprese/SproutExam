
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Infrastracture.Database.Contexts;
using Sprout.Exam.WebApp.Contracts;
using Sprout.Exam.WebApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sprout.Exam.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeDbContext employeeDbContext, IMapper mapper)
        {
            _employeeDbContext = employeeDbContext;
            _mapper = mapper;
        }

        #region COMMANDS
        public int AddEmployee(CreateEmployeeDto createEmployeeDto)
        {
            Employee newEmployee = new Employee()
            {
                FullName = createEmployeeDto.FullName,
                Birthdate = createEmployeeDto.Birthdate.Date,
                TIN = createEmployeeDto.Tin,
                EmployeeTypeId = createEmployeeDto.EmployeeTypeId
            };

            _employeeDbContext.Employee.Add(newEmployee);
            _employeeDbContext.SaveChanges();

            return newEmployee.Id;
        }

        public int DeleteEmployee(int employeeId)
        {
            Employee employee = _employeeDbContext.Employee
                .Where(e => e.Id == employeeId)
                .FirstOrDefault();

            if (employee == null) return 0;

            employee.IsDeleted = true;

            _employeeDbContext.Entry(employee).State = EntityState.Modified;
            _employeeDbContext.SaveChanges();

            return employee.Id;
        }

        public int UpdateEmployee(EditEmployeeDto editEmployeeDto)
        {
            Employee employee = _employeeDbContext.Employee
                .Where(e => e.Id == editEmployeeDto.Id)
                .FirstOrDefault();

            if (employee == null) return 0;

            employee.FullName = editEmployeeDto.FullName;
            employee.Birthdate = editEmployeeDto.Birthdate;
            employee.TIN = editEmployeeDto.Tin;
            employee.EmployeeTypeId = editEmployeeDto.EmployeeTypeId;

            _employeeDbContext.Entry(employee).State = EntityState.Modified;
            _employeeDbContext.SaveChanges();

            return employee.Id;
        }
        #endregion

        #region QUERIES
        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            var employeeList = _employeeDbContext.Employee
                .Where(e => e.IsDeleted == false)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Birthdate = e.Birthdate.Date.ToString("yyyy-MM-dd"),
                    Tin = e.TIN,
                    EmployeeTypeId = e.EmployeeTypeId
                })
                .ToList();

            return employeeList;
        }


        public EmployeeDto GetEmployeeById(int employeeId)
        {
            Employee employee = _employeeDbContext.Employee
                .Where(e => e.Id == employeeId)
                .FirstOrDefault();

            if (employee == null) return null;

            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);

            employeeDto.Birthdate = employee.Birthdate.ToString("yyyy-MM-dd");

            return employeeDto;
        }
        #endregion
    }
}
