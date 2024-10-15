using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.Departments;

namespace Company.PL.Mapping.Departments
{
    public class DepartmentProfile :Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
