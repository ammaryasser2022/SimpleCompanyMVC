using System.ComponentModel.DataAnnotations;
using Company.DAL.Models;

namespace Company.PL.ViewModels.Employees
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Is Required :( ")]
        public string Name { get; set; }

        [Range(25, 60, ErrorMessage = " Age Must Between 25 : 60 !!")]
        public int? Age { get; set; }

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address Must be like 123-Street-City-Country")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Salary Is Required :( ")]
        [DataType(DataType.Currency)]
        public int Salary { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }                                     //Dont need to view 
        public DateTime HiringDate { get; set; }                                 
        //public DateTime DateOfCreation { get; set; } = DateTime.Now;            //Dont need to view 
        public int? WorkForId { get; set; }  //Fk
        public Department? WorkFor { get; set; } // Navigational Property   

        public IFormFile? Image { get; set; }
                
        public string? ImageName { get; set; } //**** 3shan lma use Upload file - that is return an imageName 
                                               //use imageName de bt3t el Model A3mlha Map m3 imageName in Employee to be in Db  

    }
}
