using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Auth
{
    public class SignInViewModel
    {

        [EmailAddress(ErrorMessage = "Invalid Email Address :(")]
        [Required(ErrorMessage = "Email Is Required :(")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "PassWord Minimum Lenghh is 5 :(")]
        [Required(ErrorMessage = "Password Is Required :(")]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
