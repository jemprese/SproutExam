using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Contracts;
using Sprout.Exam.WebApp.Controllers;
using Sprout.Exam.WebApp.Data.Entities;
using Sprout.Exam.WebApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.UnitTest
{
    public class Tests
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private Mock<ILogger<EmployeesController>> _mockLogger;
        private Mock<IEmployee> _mockIEmployee;
        private EmployeesController _employeesController;

        [SetUp]
        public void Setup()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockIEmployee = new Mock<IEmployee>();
            _mockLogger = new Mock<ILogger<EmployeesController>>();

            _employeesController = new EmployeesController(_mockEmployeeRepository.Object, _mockLogger.Object);
        }

        #region Get All Employees Test Cases
        [Test]
        public async Task Get_EmployeeList_ReturnsOkObjectResult()
        {
            var employeeListDto = new List<EmployeeDto>
            {
                new EmployeeDto { Id = 1, FullName = "John Doe", Birthdate = "1995-03-05", Tin = "123123123", EmployeeTypeId = 1 }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetAllEmployees()).Returns(employeeListDto);

            var result = await _employeesController.Get();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(employeeListDto, okResult.Value);
        }

        [Test]
        public async Task Get_EmptyEmployeeList_ReturnsEmptyObjectResult()
        {
            _mockEmployeeRepository.Setup(repo => repo.GetAllEmployees()).Returns<List<EmployeeDto>>(null);
            
            var result = await _employeesController.Get();

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.AreEqual("Employee list is empty", notFoundResult.Value);
        }

        [Test]
        public async Task Get_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            var employeeListDto = new List<EmployeeDto>
            {
                new EmployeeDto { Id = 1, FullName = "John Doe", Birthdate = "1995-03-05", Tin = "123123123", EmployeeTypeId = 1 }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetAllEmployees()).Throws(new Exception("Simulated exception"));

            var result = await _employeesController.Get();

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(500, objectResult.StatusCode);
        }
        #endregion

        #region Get Employee By Id Test Cases
        [Test]
        public async Task GetById_ExistingId_ReturnsOkObjectResult()
        {
            int existingEmployeeId = 1;
            var existingEmployeeDto = new EmployeeDto { Id = existingEmployeeId, FullName = "John Doe", Birthdate = "1995-03-05", Tin = "123123123", EmployeeTypeId = 1 };
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(existingEmployeeId)).Returns(existingEmployeeDto);

            var result = await _employeesController.GetById(existingEmployeeId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(existingEmployeeDto, okResult.Value);
        }

        [Test]
        public async Task GetById_NonExistingId_ReturnsNotFoundResult()
        {
            int nonExistingEmployeeId = -1;
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(nonExistingEmployeeId)).Returns<EmployeeDto>(null);

            var result = await _employeesController.GetById(nonExistingEmployeeId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.AreEqual("Employee not found", notFoundResult.Value);
        }

        [Test]
        public async Task GetById_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            int existingEmployeeId = 1;
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(existingEmployeeId)).Throws(new Exception("Simulated exception"));

            var result = await _employeesController.GetById(existingEmployeeId);

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(500, objectResult.StatusCode);
        }
        #endregion

        #region Update Employee Information Test Cases
        [Test]
        public async Task Put_ValidInput_ReturnsOkObjectResult()
        {
            var input = new EditEmployeeDto { Id = 1, FullName = "John Doe", Birthdate = Convert.ToDateTime("1995-03-05").Date, Tin = "123123123", EmployeeTypeId = 1 };
            _mockEmployeeRepository.Setup(repo => repo.UpdateEmployee(input)).Returns(1);

            var result = await _employeesController.Put(input);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(1, okResult.Value);
        }

        [Test]
        public async Task Put_NoSuchEmployee_ReturnsBadRequestResult()
        {
            EditEmployeeDto input = new EditEmployeeDto { Id = -1 };
            _mockEmployeeRepository.Setup(repo => repo.UpdateEmployee(input)).Returns(0);

            var result = await _employeesController.Put(input);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Put_InvalidInput_ReturnsBadRequestResult()
        {
            var input = new EditEmployeeDto { Id = 999, FullName = "John Doe", Birthdate = Convert.ToDateTime("1995-03-05").Date, Tin = "123123123", EmployeeTypeId = 1 };
            _mockEmployeeRepository.Setup(repo => repo.UpdateEmployee(input)).Returns(0);

            var result = await _employeesController.Put(input);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual($"Unable to find employee with Id - {input.Id}", badRequestResult.Value);
        }

        [Test]
        public async Task Put_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            var input = new EditEmployeeDto { Id = 1, FullName = "John Doe", Birthdate = Convert.ToDateTime("1995-03-05").Date, Tin = "123123123", EmployeeTypeId = 1 };
            _mockEmployeeRepository.Setup(repo => repo.UpdateEmployee(input)).Throws(new Exception("Simulated exception"));

            var result = await _employeesController.Put(input);

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(500, objectResult.StatusCode);
        }
        #endregion

        #region Add New Employee Information Test Cases
        [Test]
        public async Task Post_InputIsNotEmpty_ReturnsCreatedResult()
        {
            var input = new CreateEmployeeDto { FullName = "John Doe", Birthdate = Convert.ToDateTime("1995-03-05").Date, Tin = "123123123", EmployeeTypeId = 1 };
            _mockEmployeeRepository.Setup(repo => repo.AddEmployee(input)).Returns(1); // Simulate successful update

            var result = await _employeesController.Post(input);

            Assert.IsInstanceOf<CreatedResult>(result);
            var okResult = (CreatedResult)result;
            Assert.AreEqual(1, okResult.Value);
        }

        [Test]
        public async Task Post_InputIsEmpty_ReturnsBadRequestResult()
        {
            CreateEmployeeDto input = null;
            _mockEmployeeRepository.Setup(repo => repo.AddEmployee(It.IsAny<CreateEmployeeDto>())).Returns(null);

            var result = await _employeesController.Post(input);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("Employee data is null", badRequestResult.Value);
        }

        [Test]
        public async Task Post_InvalidValue_ReturnsBadRequestResult()
        {
            CreateEmployeeDto input = new CreateEmployeeDto(); // Create an empty CreateEmployeeDto to trigger invalid ModelState
            _employeesController.ModelState.AddModelError("FullName", "Employee name is required"); // Add a ModelState error

            // Act
            var result = await _employeesController.Post(input);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOf<SerializableError>(badRequestResult.Value);

            var errors = (SerializableError)badRequestResult.Value;
            Assert.IsTrue(errors.ContainsKey("FullName"));
            Assert.AreEqual("Employee name is required", ((string[])errors["FullName"])[0]);
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsInternalServerErrorResult()
        {
            CreateEmployeeDto input = new CreateEmployeeDto { FullName = "John Doe", Birthdate = Convert.ToDateTime("1995-03-05").Date, Tin = "123123123", EmployeeTypeId = 1 };
            _mockEmployeeRepository.Setup(repo => repo.AddEmployee(input)).Throws(new Exception("Simulated exception"));

            var result = await _employeesController.Post(input);

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(500, objectResult.StatusCode);
        }
        #endregion

        #region Delete Employee Information Test Cases
        [Test]
        public async Task Delete_ValidId_ReturnsOkResult()
        {
            int validEmployeeId = 1;
            _mockEmployeeRepository.Setup(repo => repo.DeleteEmployee(validEmployeeId)).Returns(validEmployeeId);

            var result = await _employeesController.Delete(validEmployeeId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(validEmployeeId, okResult.Value);
        }

        [Test]
        public async Task Delete_InvalidId_ReturnsBadRequestResult()
        {
            int invalidEmployeeId = -1;
            _mockEmployeeRepository.Setup(repo => repo.DeleteEmployee(invalidEmployeeId)).Returns(0);

            var result = await _employeesController.Delete(invalidEmployeeId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Delete_NoSuchEmployee_ReturnsBadRequestResult()
        {
            int nonExistingEmployeeId = -1;
            _mockEmployeeRepository.Setup(repo => repo.DeleteEmployee(nonExistingEmployeeId)).Returns(0);

            var result = await _employeesController.Delete(nonExistingEmployeeId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Delete_ExceptionThrown_ReturnsStatusCode500()
        {
            int validEmployeeId = 1;
            _mockEmployeeRepository.Setup(repo => repo.DeleteEmployee(validEmployeeId)).Throws(new Exception());

            var result = await _employeesController.Delete(validEmployeeId);

            Assert.IsInstanceOf<ObjectResult>(result);
            var statusCodeResult = (ObjectResult)result;
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task Delete_ValidId_DeletesEmployee()
        {
            int validEmployeeId = 1;
            _mockEmployeeRepository.Setup(repo => repo.DeleteEmployee(validEmployeeId)).Returns(validEmployeeId);

            await _employeesController.Delete(validEmployeeId);

            _mockEmployeeRepository.Verify(repo => repo.DeleteEmployee(validEmployeeId), Times.Once);
        }
        #endregion

        #region Calculate Salary of Employee
        [Test]
        public async Task Calculate_Salary_ValidInputAndRegularEmployeeFound_ReturnsOkResult()
        {
            CalculateSalaryDto calculateSalaryDto = new CalculateSalaryDto { Id = 1, absentDays = 2, workedDays = 0 };
            EmployeeDto employee = new EmployeeDto { Id = 1, FullName = "John Doe", Birthdate = "1995-03-05", Tin = "123123123", EmployeeTypeId = 1 };

            decimal expectedSalary = 16647.62m;
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(calculateSalaryDto.Id)).Returns(employee);

            _mockIEmployee.Setup(factory => factory.CalculateSalary(calculateSalaryDto.absentDays, calculateSalaryDto.workedDays)).Returns(expectedSalary);

            var result = await _employeesController.Calculate(calculateSalaryDto) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSalary, result.Value);
        }

        [Test]
        public async Task Calculate_Salary_ValidInputAndContractualEmployeeFound_ReturnsOkResult()
        {
            CalculateSalaryDto calculateSalaryDto = new CalculateSalaryDto { Id = 1, absentDays = 0, workedDays = 15.5m };
            EmployeeDto employee = new EmployeeDto { Id = 1, FullName = "John Doe", Birthdate = "1995-03-05", Tin = "123123123", EmployeeTypeId = 2 };

            decimal expectedSalary = 7750.00m;
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(calculateSalaryDto.Id)).Returns(employee);

            _mockIEmployee.Setup(factory => factory.CalculateSalary(calculateSalaryDto.absentDays, calculateSalaryDto.workedDays)).Returns(expectedSalary);

            var result = await _employeesController.Calculate(calculateSalaryDto) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedSalary, result.Value);
        }

        [Test]
        public async Task Calculate_Salary_ValidInputAndEmployeeNotFound_ReturnsNotFoundResult()
        {
            CalculateSalaryDto calculateSalaryDto = new CalculateSalaryDto { Id = 1, absentDays = 0, workedDays = 15.5m };
            EmployeeDto employee = null;
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(calculateSalaryDto.Id)).Returns(employee);

            var result = await _employeesController.Calculate(calculateSalaryDto) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        #endregion
    }
}