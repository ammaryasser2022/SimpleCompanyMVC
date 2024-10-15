using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Auth
{
    public class ForgetPassswordViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email Address :(")]
        [Required(ErrorMessage = "Email Is Required :(")]
        public string? Email { get; set; }
    }
}
