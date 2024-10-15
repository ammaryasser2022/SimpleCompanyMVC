using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.RoleManager
{
    public class RoleViewModel
    {
        public string? Id { get; set; }


        [Required(ErrorMessage = "Name Is Required :( ")]
        public string RoleName { get; set; }

    }
}
