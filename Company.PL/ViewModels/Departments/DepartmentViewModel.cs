using Company.DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Departments
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required :( ")]

        public string Code { get; set; }


        [Required(ErrorMessage = "Name is Required :( ")]
        public string Name { get; set; }

        [DisplayName("Date Of Creation ")]  // for @Html.DisplayNameFor(D => D.DateOfCreation)
        public DateTime DateOfCreation { get; set; }


        //public ICollection<Employee>? Employees { get; set; }
    }
}
