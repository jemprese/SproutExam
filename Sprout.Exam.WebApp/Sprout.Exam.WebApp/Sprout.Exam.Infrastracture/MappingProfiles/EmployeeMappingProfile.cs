using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Infrastracture.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile() 
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Birthdate.Date.ToString("yyyy-MM-dd")));

            CreateMap<EmployeeDto, Employee>();
        }
    }
}
