using System.ComponentModel.DataAnnotations;

namespace IVoive_WEB.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
