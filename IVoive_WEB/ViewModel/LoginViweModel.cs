using System.ComponentModel.DataAnnotations;

namespace IVoive_WEB.ViewModel
{
    public class LoginViweModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
