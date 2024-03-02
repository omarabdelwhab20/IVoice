using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IVoice.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
    }
}
