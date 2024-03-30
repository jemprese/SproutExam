using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class CreateEmployeeDto: BaseSaveEmployeeDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        public override string FullName { get; set; }

        [Required(ErrorMessage = "Employee TIN is required")]
        public override string Tin { get; set; }

        [MinimumAge(18, ErrorMessage = "Employee must be 18 years old or above.")]
        public override DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Employee Type is required")]
        public override int EmployeeTypeId { get; set; }
    }

    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is DateTime birthdate)
            {
                if(birthdate.AddYears(_minimumAge) > DateTime.Today) 
                {
                    return new ValidationResult($"Employee must be {_minimumAge} years old or above.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date format.");
        }
    }
}
