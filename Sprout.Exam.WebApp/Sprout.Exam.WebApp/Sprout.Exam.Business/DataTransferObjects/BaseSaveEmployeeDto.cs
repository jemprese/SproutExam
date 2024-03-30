using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public abstract class BaseSaveEmployeeDto
    {
        public virtual string FullName { get; set; }
        public virtual string Tin { get; set; }
        public virtual DateTime Birthdate { get; set; }
        public virtual int EmployeeTypeId { get; set; }
    }
}
