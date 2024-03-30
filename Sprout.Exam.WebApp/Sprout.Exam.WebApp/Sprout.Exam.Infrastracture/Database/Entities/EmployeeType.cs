using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Sprout.Exam.WebApp.Data.Entities
{
    public class EmployeeType
    {
        [Key]
        [DisallowNull]
        public int Id { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string TypeName { get; set; }
        public ICollection<Employee> Employee { get; set;}
    }
}
