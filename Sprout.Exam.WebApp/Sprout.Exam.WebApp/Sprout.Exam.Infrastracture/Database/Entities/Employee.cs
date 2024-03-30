using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Sprout.Exam.WebApp.Data.Entities
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [DisallowNull]
        public int Id { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string FullName { get; set; }
        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string TIN { get; set; }
        public int EmployeeTypeId { get; set; }
        public EmployeeType EmployeeType { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

    }
}
