using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class EditEmployeeDto: CreateEmployeeDto
    {
        public int Id { get; set; }
    }
}
