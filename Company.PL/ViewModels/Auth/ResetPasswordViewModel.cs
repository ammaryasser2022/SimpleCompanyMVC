using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Auth
{
    public class ResetPasswordViewModel
    {

        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "PassWord Minimum Lenghh is 5 :(")]
        [Required(ErrorMessage = "Password Is Required :(")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "PassWord Minimum Lenghh is 5 :(")]
        [Required(ErrorMessage = "ConfirmPassword Is Required :(")]
        [Compare(nameof(Password), ErrorMessage = "Passwords Not Matched :(")]
        public string ConfirmPassword { get; set; }
    }
}
