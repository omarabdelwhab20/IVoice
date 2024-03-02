using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IVoive_WEB.Models
{
    public class ApplicationUser : IdentityUser
    {


        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
