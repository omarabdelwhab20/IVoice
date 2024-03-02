using System.ComponentModel.DataAnnotations;

namespace IVoice.ViewModel
{
    public class LoginViweModel
    {
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
