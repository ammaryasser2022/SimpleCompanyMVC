using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Auth
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "UserName Is Required :(")]
        public string UserName { get; set; }



        [Required(ErrorMessage = "FirstName Is Required :(")]
        public string FirstName { get; set; }



        [Required(ErrorMessage = "LastName Is Required :(")]
        public string LastName { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address :(")]
        [Required(ErrorMessage = "Email Is Required :(")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "PassWord Minimum Lenghh is 5 :(")]
        [Required(ErrorMessage = "Password Is Required :(")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "PassWord Minimum Lenghh is 5 :(")]
        [Required(ErrorMessage = "ConfirmPassword Is Required :(")]
        [Compare(nameof(Password), ErrorMessage = "Passwords Not Matched :(")]
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessage = "Agreement Is Required :(")]
        public bool IsAgree { get; set; }
    }
}
