using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Company.DAL.Models
{
    public class ApplicationUser : IdentityUser // take the prop in IdentityUser and addd FirstName  , LastName
    {
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public bool IsAgree { get; set; }
    }
}
